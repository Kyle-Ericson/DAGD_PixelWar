using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class GameScene : ESingletonMono<GameScene>
{
    private GameObject unitPrefab;
    private SelectionBox cursor;
    private Unit _currentSelected = null;
    public Unit currentSelected
    {
        get { return _currentSelected; }
    }
    public GameState _gameState = GameState.awaitingInput;
    public GameState gameState
    {
        get { return _gameState; }
    }
    private GameUI gameUI = null;
    private EMenu actionMenu = null;
    private Vector2 tempGridPos;
    public bool running = true;
    private float zoffset = -0.2f;
    private int numOfPlayers = 0;
    private int currentTurn = 1;
    private int turnCount = 1;
    private UnitType nextSplitType;
    


    


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
        SpawnUnit(UnitType.worker, Team.Player1, MapManager.ins.currentMapData.start1.ToVector2());
        SpawnUnit(UnitType.worker, Team.Player2, MapManager.ins.currentMapData.start2.ToVector2());
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
    //
    private void HandleMouseLClick()
    {
        switch (_gameState)
        {
            case GameState.awaitingInput:
                if (MapManager.ins.unitGrid.ContainsKey(cursor.gridpos) && MapManager.ins.unitGrid[cursor.gridpos].state != UnitState.sleeping)
                {
                    if(MapManager.ins.unitGrid[cursor.gridpos].team == (Team)currentTurn) SelectUnit();
                }
                else DeselectAll();
                break;

            case GameState.unitSelected:
                if (_currentSelected.inMoveRange.Contains(cursor.gridpos))
                {
                    if(cursor.gridpos == _currentSelected.gridpos)
                    {
                        _currentSelected.SetPrevPos();
                        TempMoveUnit();
                    }
                    else 
                    {
                        _currentSelected.LerpToPosition();
                        ChangeState(GameState.animating);
                    }
                }
                break;

            case GameState.awaitingAction:
            
                break;

            case GameState.awaitingSplit:
                if(currentSelected.inSplitRange.Contains(cursor.gridpos))
                {
                    ConfirmSplit(cursor.gridpos);
                }
                break;

            case GameState.awaitingAttack:
                if(currentSelected.inAttackRange.Contains(cursor.gridpos))
                {
                    ConfirmAttack(cursor.gridpos);
                    DeselectAll();
                }
                break;
            case GameState.awaitingEat:
                if(currentSelected.foodInRange.Contains(cursor.gridpos))
                {
                    ConfirmEat(cursor.gridpos);
                    DeselectAll();
                }
                break;
                
            case GameState.paused:
                break;
        }
    }
    //
    private void HandleMouseRClick()
    {
        switch(_gameState)
        {
            case GameState.awaitingInput:
                DeselectAll();
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
            case GameState.unitSelected:
                DeselectAll();
                break;
            case GameState.awaitingAction:
                Undo();
                DeselectAll();
                break;
            case GameState.awaitingAttack:
                DeselectAll();
                break;
            case GameState.awaitingEat:
                DeselectAll();
                break;
            case GameState.awaitingSplit:
                DeselectAll();
                break;
            case GameState.paused:
                break;
        }
    }
    #endregion
    #region Unit Control
    //
    private void SpawnUnit(UnitType type, Team team, Vector2 gridPos)
    {
        Vector3 worldPos = MapManager.ins.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitPrefab).GetComponent<Unit>();
        unit.gameObject.transform.position = worldPos;
        unit.Init(type, team);
        MapManager.ins.unitGrid.Add(gridPos, unit);
    }
    //
    private void SelectUnit()
    {
        _currentSelected = MapManager.ins.unitGrid[cursor.gridpos];
        _currentSelected.Select();
        _currentSelected.CheckMove();
        foreach(Vector2 v in currentSelected.inMoveRange) MapManager.ins.currentMap[v].Highlight();
        ChangeState(GameState.unitSelected);
    }
    private void ShowPath()
    {
        foreach (Vector2 v in currentSelected.inMoveRange) MapManager.ins.currentMap[v].SetIconColor(Color.white);
        if (GameScene.ins.currentSelected.inMoveRange.Contains(cursor.gridpos))
        {
            var start = MapManager.ins.WorldToGrid(GameScene.ins.currentSelected.transform.position);
            AStar.ins.FindPath(start, cursor.gridpos);
        }
    }
    private void ConfirmSplit(Vector2 splitPos)
    {
        SpawnUnit(nextSplitType, currentSelected.team, splitPos);
        MapManager.ins.unitGrid[splitPos].Sleep();
        currentSelected.Split();
        currentSelected.Sleep();
        ConfirmMove();        
    }
    private void ConfirmAttack(Vector2 attackPos)
    {
        var attacked = MapManager.ins.unitGrid[attackPos];
        attacked.TakeDamage(currentSelected.data.attack);
        if(attacked.state == UnitState.dead) KillUnit(attackPos);
        currentSelected.Sleep();
        ConfirmMove();
    }
    private void ConfirmEat(Vector2 eatPos)
    {
        currentSelected.Eat();
        currentSelected.Sleep();
        ConfirmMove();
    }
    private void KillUnit(Vector2 unitPos)
    {
        MapManager.ins.RemoveUnit(unitPos);
    }
    //
    public void TempMoveUnit()
    {   
        _currentSelected.Move(cursor.gridpos);
        _currentSelected.CheckAttackRange();
        _currentSelected.CheckForEnemies();
        _currentSelected.CheckForFood();
        _currentSelected.CheckSplitRange();
        _currentSelected.AwaitAction();
        DeselectAllTiles();
        
        // setup the action menu based on what is around the unit
        actionMenu.transform.position = cursor.transform.position;
        actionMenu.Clear();
        actionMenu.AddRadialWait().onClick.AddListener(HandleWait);
        if (_currentSelected.enemiesInRange.Count > 0) actionMenu.AddRadialAttack().onClick.AddListener(HandleAttack);
        if (_currentSelected.foodInRange.Count > 0) actionMenu.AddRadialEat().onClick.AddListener(HandleEat);
        if (_currentSelected.IsFoodMaxed()) actionMenu.AddRadialSplit().onClick.AddListener(HandleSplit);
        actionMenu.UpdateRadialMenu();
        

        // change the game state to wait for an action to be selected
        ChangeState(GameState.awaitingAction);
    }
    private void ConfirmMove()
    {
        _currentSelected.SetGridPos();
        MapManager.ins.unitGrid.Remove(_currentSelected.prevPos);
        MapManager.ins.unitGrid.Add(_currentSelected.gridpos, _currentSelected);
        _currentSelected = null;
        DeselectAll();
    }
    //
    private void Undo()
    {
        _currentSelected.Undo();
        ChangeState(GameState.awaitingInput);
        actionMenu.Clear();
    }
    #endregion
    #region Action Menu Handlers
    //
    public void HandleWait()
    {
        _currentSelected.Sleep();
        ConfirmMove();
    }
    //
    public void HandleAttack()
    {
        foreach(Vector2 v in _currentSelected.enemiesInRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight();
            tile.SetIconColor(Color.red / 2);
        }
        ChangeState(GameState.awaitingAttack);
    }
    //
    public void HandleEat()
    {
        foreach(Vector2 v in _currentSelected.foodInRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight();
            tile.SetIconColor(Color.green);
        }
        ChangeState(GameState.awaitingEat);
    }
    //
    public void HandleSplit()
    {
        
        actionMenu.Clear();
        actionMenu.AddRadialWorker().onClick.AddListener(() => 
        { 
            nextSplitType = UnitType.worker;
            ChangeState(GameState.awaitingSplit);
        });
        actionMenu.AddRadialTank().onClick.AddListener(() => 
        { 
            nextSplitType = UnitType.tank; 
            ChangeState(GameState.awaitingSplit);
        });
        actionMenu.AddRadialInfantry().onClick.AddListener(() => 
        { 
            nextSplitType = UnitType.infantry; 
            ChangeState(GameState.awaitingSplit);
        });
        actionMenu.AddRadialSniper().onClick.AddListener(() => 
        { 
            nextSplitType = UnitType.sniper;
            ChangeState(GameState.awaitingSplit);
        });
        actionMenu.UpdateRadialMenu();
    }
    #endregion
    #region Game Flow
    //
    public void ChangeState(GameState newState)
    {
        _gameState = newState;
        switch (_gameState)
        {
            case GameState.awaitingInput:
                InputEvents.ins.OnMouseMoved -= ShowPath;
                cursor.Show();
                actionMenu.Hide();
                break;
            case GameState.unitSelected:
                InputEvents.ins.OnMouseMoved += ShowPath;
                break;
            case GameState.awaitingAction:
                AddListeners();
                InputEvents.ins.OnMouseMoved -= ShowPath;
                cursor.Hide();
                actionMenu.Show();
                break;
            case GameState.awaitingSplit:
                InputEvents.ins.OnMouseMoved -= ShowPath;
                foreach (Vector2 v in currentSelected.inSplitRange)
                {
                    Tile tile = MapManager.ins.currentMap[v];
                    tile.Highlight();
                    tile.SetIconColor(Color.yellow);
                }
                cursor.Show();
                actionMenu.Hide();
                break;
            case GameState.awaitingAttack:
                InputEvents.ins.OnMouseMoved -= ShowPath;
                cursor.Show();
                actionMenu.Hide();
                break;
            case GameState.awaitingEat:
                cursor.Show();
                actionMenu.Hide();
                break;
            case GameState.animating:
                RemoveListeners();
                InputEvents.ins.OnMouseMoved -= ShowPath;
                cursor.Hide();
                actionMenu.Hide();
                DeselectAllTiles();
                break;
            case GameState.paused:
                cursor.Hide();
                break;
        }
    }
    //
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
    }
    #endregion
    #region Helpers
    //
    public void WakeUpAll()
    {
        foreach (KeyValuePair<Vector2, Unit> item in MapManager.ins.unitGrid)
        {
            item.Value.Idle();
        }
    }
    //
    private void DeselectAll()
    {
        DeselectAllTiles();
        DeselectAllUnits();
        ChangeState(GameState.awaitingInput);
    }
    //
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap) 
        { 
            k.Value.UnHighlight();
            k.Value.SetIconColor(Color.white);
        }
    }
    //
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            if (k.Value.state != UnitState.sleeping) k.Value.Deselect();
        }
        _currentSelected = null;
    }
    
    //
    private void RemoveAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> item in MapManager.ins.currentMap) { Destroy(item.Value.gameObject); }
        MapManager.ins.currentMap.Clear();
    }
    //
    private void RemoveAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> item in MapManager.ins.unitGrid) { Destroy(item.Value.gameObject); }
    }
    //
    private void ClearScene()
    {
        RemoveAllTiles();
        RemoveAllUnits();
        RemoveListeners();
        cursor.Hide();
    }
    #endregion


}



