using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class GameScene : ESingletonMono<GameScene>
{
    public GameObject unitPrefab;
    public SelectionBox cursor;
    public Unit _currentSelected = null;
    public GameState _gameState = GameState.awaitingInput;
    public GameUI gameUI = null;
    public EMenu actionMenu = null;
    public Vector2 tempGridPos;
    public bool running = true;
    private float zoffset = -0.2f;
    public int numOfPlayers = 0;
    public int currentTurn = 1;
    public int turnCount = 1;
    private UnitType nextSplitType;



    public Unit currentSelected
    {
        get { return _currentSelected; }
    }
    public GameState gameState
    {
        get { return _gameState; }
    }
    


    


    #region Game Setup
    //
    public void Init()
    {
        // load and setup prefabs
        cursor = Instantiate(Resources.Load<GameObject>("prefabs/Cursor").GetComponent<SelectionBox>());
        gameUI = Instantiate(Resources.Load<GameObject>("prefabs/GameUI").GetComponent<GameUI>());
        actionMenu = Instantiate(Resources.Load<GameObject>("prefabs/RadialMenu")).GetComponentInChildren<EMenu>();
        unitPrefab = Resources.Load<GameObject>("prefabs/Unit");
        // set this object as each componenets parent
        gameUI.gameObject.transform.SetParent(this.gameObject.transform);
        actionMenu.gameObject.transform.SetParent(this.gameObject.transform);
        MapManager.ins.gameObject.transform.SetParent(this.gameObject.transform);
        InputEvents.ins.gameObject.transform.SetParent(this.gameObject.transform);
        AStar.ins.gameObject.transform.SetParent(this.gameObject.transform);
        // load the sprites and load the database
        Sprites.ins.LoadSprites();
        Database.Load();
    }
    public void SetupMatch(int map, int players)
    {
        LoadMap(map);
        cursor.Show();
        cursor.Move(Vector2.zero);
        AddListeners();
        numOfPlayers = players;
    }
    //
    private void LoadMap(int mapID)
    {
        MapManager.ins.SpawnMap(mapID);
        SpawnUnit(UnitType.original, Team.Player1, MapManager.ins.currentMapData.start1.ToVector2());
        SpawnUnit(UnitType.original, Team.Player2, MapManager.ins.currentMapData.start2.ToVector2());
    }
    #endregion

    
    #region Input Handling
    //
    private void AddListeners()
    {
        InputEvents.ins.OnMouseLClick += HandleMouseLClick;
        InputEvents.ins.OnMouseRClick += HandleMouseRClick;
        InputEvents.ins.OnMouseMoved += cursor.Move;
    }
    private void RemoveListeners()
    {
        InputEvents.ins.OnMouseLClick -= HandleMouseLClick;
        InputEvents.ins.OnMouseRClick -= HandleMouseRClick;
        InputEvents.ins.OnMouseMoved -= cursor.Move;
    }

    // handle a left mouse button click
    private void HandleMouseLClick()
    {
        switch (_gameState)
        {
            case GameState.awaitingInput:
                if (MapManager.ins.unitGrid.ContainsKey(cursor.gridpos) && 
                    MapManager.ins.unitGrid[cursor.gridpos].state != UnitState.sleeping)
                {
                    if(MapManager.ins.unitGrid[cursor.gridpos].team == (Team)currentTurn) SelectUnit();
                }
                else DeselectAll();
                break;

            case GameState.unitSelected:
                
                if (_currentSelected.inMoveRange.Contains(cursor.gridpos))
                {
                    _currentSelected.LerpToPosition();
                    StateAnimating();
                }

                break;
            case GameState.awaitingAction:
            
                break;
            case GameState.awaitingSplit:
                if(currentSelected.inSplitRange.Contains(cursor.gridpos)) ConfirmSplit(cursor.gridpos);
                break;

            case GameState.awaitingAttack:
                if(currentSelected.inAttackRange.Contains(cursor.gridpos)) ConfirmAttack(cursor.gridpos);
                break;
                
            case GameState.awaitingEat:
                if(currentSelected.foodInRange.Contains(cursor.gridpos)) ConfirmEat(cursor.gridpos);
                break;
            case GameState.paused:
                break;
        }
    }

    // handle when the right mouse button is clicked
    private void HandleMouseRClick()
    {

        switch(_gameState)
        {
            case GameState.awaitingInput:
                if (MapManager.ins.unitGrid.ContainsKey(cursor.gridpos))
                {
                    var unit = MapManager.ins.unitGrid[cursor.gridpos];
                    unit.CheckAttackRange();

                    foreach(Vector2 v in unit.inAttackRange)
                    {
                        Tile tile = MapManager.ins.currentMap[v];
                        tile.Highlight();
                        tile.SetIconColor(Color.red);
                    }
                }
                break;
            default:
                Undo();
                break;
        }
    }
    #endregion


    #region Action Confirmation

    // confirm the split action
    private void ConfirmSplit(Vector2 splitPos)
    {
        SpawnUnit(nextSplitType, currentSelected.team, splitPos);
        MapManager.ins.unitGrid[splitPos].Sleep();
        currentSelected.Split();
        ConfirmMove();        
    }

    // confirm the attack action
    private void ConfirmAttack(Vector2 attackPos)
    {
        var attacked = MapManager.ins.unitGrid[attackPos];
        attacked.TakeDamage(currentSelected.data.attack);
        if(attacked.state == UnitState.dead) KillUnit(attackPos);
        ConfirmMove();
    }

    // confirm the eat action
    private void ConfirmEat(Vector2 eatPos)
    {
        currentSelected.Eat();
        ConfirmMove();
    }

    // confirm the temporary move
    private void ConfirmMove()
    {
        MapManager.ins.unitGrid.Remove(_currentSelected.prevPos);
        currentSelected.Sleep();
        MapManager.ins.unitGrid.Add(_currentSelected.gridpos, _currentSelected);
        _currentSelected = null;
        DeselectAll();
        UpdateFog();
    }
    #endregion


    #region Action Menu Handlers

    // handle wait button
    public void HandleWait()
    {
        ConfirmMove();
    }

    // handle attack button
    public void HandleAttack()
    {
        foreach(Vector2 v in _currentSelected.enemiesInRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight();
            tile.SetIconColor(Color.red / 2);
        }
        StateAwaitAttack();
    }

    // handle eat button
    public void HandleEat()
    {
        foreach(Vector2 v in _currentSelected.foodInRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight();
            tile.SetIconColor(Color.green);
        }
        StateAwaitEat();
    }

    // handle the split button being clicked
    public void HandleSplit()
    {
        actionMenu.Clear();
        
        // add worker button
        if(_currentSelected.food == Database.unitData[(int)UnitType.scout].cost)
        {
            actionMenu.AddRadialScout().onClick.AddListener(() => 
            { 
                nextSplitType = UnitType.scout;
                StateAwaitSplit();
            });
        }
        // add tank button
        if(_currentSelected.food == Database.unitData[(int)UnitType.tank].cost)
        {
            actionMenu.AddRadialTank().onClick.AddListener(() => 
            { 
                nextSplitType = UnitType.tank; 
                StateAwaitSplit();
            });    
        }
        // add infantry button
        if(_currentSelected.food == Database.unitData[(int)UnitType.infantry].cost)
        {
            actionMenu.AddRadialInfantry().onClick.AddListener(() => 
            { 
                nextSplitType = UnitType.infantry; 
                StateAwaitSplit();
            });
        }
        // add sniper
        if(_currentSelected.food == Database.unitData[(int)UnitType.sniper].cost)
        {
            actionMenu.AddRadialSniper().onClick.AddListener(() => 
            { 
                nextSplitType = UnitType.sniper;
                StateAwaitSplit();
            });
        }
        // add sniper
        if(_currentSelected.food == Database.unitData[(int)UnitType.original].cost)
        {
            actionMenu.AddRadialOriginal().onClick.AddListener(() => 
            { 
                nextSplitType = UnitType.original;
                StateAwaitSplit();
            });
        }
        
        actionMenu.UpdateRadialMenu();
    }
    #endregion


    #region State Managers

    public void StateAwaitInput()
    {
        InputEvents.ins.OnMouseMoved -= ShowPath;
        cursor.Show();
        actionMenu.Hide();
        _gameState = GameState.awaitingInput;
    }
    public void StateUnitSelected()
    {
        InputEvents.ins.OnMouseMoved += ShowPath;
        _gameState = GameState.unitSelected;
    }
    public void StateAwaitAction()
    {
        AddListeners();
        InputEvents.ins.OnMouseMoved -= ShowPath;
        cursor.Hide();
        actionMenu.Show();
        _gameState = GameState.awaitingAction;
    }
    public void StateAwaitSplit()
    {
        InputEvents.ins.OnMouseMoved -= ShowPath;
        foreach (Vector2 v in currentSelected.inSplitRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight();
            tile.SetIconColor(Color.yellow);
        }
        cursor.Show();
        actionMenu.Hide();
        _gameState = GameState.awaitingSplit;
    }
    public void StateAwaitAttack()
    {
        InputEvents.ins.OnMouseMoved -= ShowPath;
        cursor.Show();
        actionMenu.Hide();
        _gameState = GameState.awaitingAttack;
    }
    public void StateAwaitEat()
    {
        cursor.Show();
        actionMenu.Hide();
        _gameState = GameState.awaitingEat;
    }
    public void StateAnimating()
    {
        RemoveListeners();
        InputEvents.ins.OnMouseMoved -= ShowPath;
        cursor.Hide();
        actionMenu.Hide();
        DeselectAllTiles();
        _gameState = GameState.animating;
    }
    public void StatePaused()
    {
        RemoveListeners();
        cursor.Hide();
        _gameState = GameState.paused;
    }

    #endregion


    #region Tile Controls

    // wake up all units
    public void WakeUpAll()
    {
        foreach (KeyValuePair<Vector2, Unit> item in MapManager.ins.unitGrid)
        {
            item.Value.Idle();
        }
    }

    // deselected everything
    private void DeselectAll()
    {
        DeselectAllTiles();
        DeselectAllUnits();
        StateAwaitInput();
    }

    // Make sure there are no highlighted tiles
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap) 
        { 
            k.Value.SetIconColor(Color.white);
            k.Value.UnHighlight();
        }
    }

    // make sure there are no units selected
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            if (k.Value.state != UnitState.sleeping) k.Value.Deselect();
        }
        _currentSelected = null;
    }

    // remove all tiles from the scene
    private void RemoveAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> item in MapManager.ins.currentMap) { Destroy(item.Value.gameObject); }
        MapManager.ins.currentMap.Clear();
    }

    // remove all units from the scene
    private void RemoveAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> item in MapManager.ins.unitGrid) { Destroy(item.Value.gameObject); }
    }

    // this method cleans up the scene
    private void ClearScene()
    {
        RemoveAllTiles();
        RemoveAllUnits();
        RemoveListeners();
        cursor.Hide();
    }

    #endregion


    #region Unit Controls

    // temporary unit move, can still be undon
    public void TempMoveUnit()
    {
        _currentSelected.CheckEverything();
        _currentSelected.AwaitAction();
        DeselectAllTiles();
        SetupActionMenu();
        StateAwaitAction();
    }

    // setup the action menu based on what is around the unit
    private void SetupActionMenu()
    {
        actionMenu.transform.position = cursor.transform.position;
        actionMenu.Clear();
        actionMenu.AddRadialWait().onClick.AddListener(HandleWait);
        
        if(_currentSelected.data.type == UnitType.original)
        {
            if (_currentSelected.foodInRange.Count > 0) actionMenu.AddRadialEat().onClick.AddListener(HandleEat);
            if (_currentSelected.food > 0 && _currentSelected.inSplitRange.Count > 0) actionMenu.AddRadialSplit().onClick.AddListener(HandleSplit);
        }
        else
        {
            actionMenu.AddRadialAttack().onClick.AddListener(HandleAttack);
        }
        actionMenu.UpdateRadialMenu();
    }

    // destroy the unit
    private void KillUnit(Vector2 unitPos)
    {
        MapManager.ins.RemoveUnit(unitPos);
    }
    
    //
    private void SpawnUnit(UnitType type, Team team, Vector2 gridPos)
    {
        Vector3 worldPos = MapManager.ins.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitPrefab).GetComponent<Unit>();
        unit.gameObject.transform.position = worldPos;
        unit.Init(type, team);
        MapManager.ins.unitGrid.Add(gridPos, unit);
        unit.SetPrevPos();
        unit.SetGridPos();
        UpdateFog();
    }

    //
    private void SelectUnit()
    {
        _currentSelected = MapManager.ins.unitGrid[cursor.gridpos];
        _currentSelected.Select();
        _currentSelected.CheckMove();
        if(_currentSelected.inMoveRange.Count > 0)
        {
            foreach(Vector2 v in currentSelected.inMoveRange) 
            {
                if(v != _currentSelected.gridpos) MapManager.ins.currentMap[v].Highlight();
            }
        }
        StateUnitSelected();
    }

    #endregion



    // OnMouseMoved listener
    // when the mouse moves show the path that will be followed
    private void ShowPath()
    {
        if(_currentSelected.inMoveRange.Count < 2) return;
        foreach (Vector2 v in currentSelected.inMoveRange) MapManager.ins.currentMap[v].SetIconColor(Color.white);
        if (_currentSelected.inMoveRange.Contains(cursor.gridpos))
        {
            var start = _currentSelected.gridpos;
            AStar.ins.FindPath(start, cursor.gridpos);
        }
        
    }

    //
    private void UpdateFog()
    {
        AllFogOn();
        AllUnitsOff();

        foreach(KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            if(k.Value.team == (Team)currentTurn)
            {
                k.Value.Show();
                k.Value.CheckVision();
                foreach(Vector2 v in k.Value.visibleTiles)
                {
                    MapManager.ins.currentMap[v].HideFog();
                    if(MapManager.ins.unitGrid.ContainsKey(v))
                    {
                        MapManager.ins.unitGrid[v].Show();
                    }
                }
            }
        }
    }

    //
    private void AllFogOn()
    {
        foreach(KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap)
        {
            k.Value.ShowFog();
        }
    }
    //
    private void AllUnitsOff()
    {
        foreach(KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            k.Value.Hide();
        }
    }

    // end the turn
    public void EndTurn()
    {
        currentTurn++;
        if (currentTurn > numOfPlayers)
        {
            currentTurn = 1;
            turnCount++;
        }
        DeselectAll();
        WakeUpAll();
        StateAwaitInput();
        UpdateFog();
    }

    // undo the last move set everything back to await input
    private void Undo()
    {
        if(_currentSelected) _currentSelected.Undo();
        DeselectAll();
        AStar.ins.ClearPath();
        actionMenu.Clear();
    }

    

}



