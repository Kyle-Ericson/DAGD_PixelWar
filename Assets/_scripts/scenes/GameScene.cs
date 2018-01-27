using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class GameScene : ESingletonMono<GameScene>
{
    private GameObject unitPrefab;
    private Cursor cursor;
    private Unit _currentSelected = null;
    public Unit currentSelected
    {
        get { return _currentSelected; }
    }
    private GameState _gameState = GameState.awaitingInput;
    public GameState gameState
    {
        get { return _gameState; }
    }
    private GameUI gameUI = null;
    private Vector2 tempGridPos;
    public bool running = true;
    private float zoffset = -0.2f;
    private int numOfPlayers = 0;
    private int currentTurn = 1;
    private int turnCount = 1;
    


    


    #region Game Setup
    //
    public void Init()
    {
        cursor = Instantiate(Resources.Load<GameObject>("prefabs/Cursor").GetComponent<Cursor>());
        gameUI = Instantiate(Resources.Load<GameObject>("prefabs/GameUI").GetComponent<GameUI>());
        unitPrefab = Resources.Load<GameObject>("prefabs/Unit");
        gameUI.gameObject.transform.SetParent(this.gameObject.transform);
        MapManager.ins.gameObject.transform.SetParent(this.gameObject.transform);
        InputEvents.ins.gameObject.transform.SetParent(this.gameObject.transform);
        AStar.ins.gameObject.transform.SetParent(this.gameObject.transform);
        Sprites.ins.LoadSprites();
        Database.Load();
    }
    public void SetupMatch(int map, int players)
    {
        LoadMap(map);
        cursor.Show();
        AddLIsteners();
        numOfPlayers = players;
    }
    //
    private void LoadMap(int mapID)
    {
        MapManager.ins.SpawnMap(mapID);
        SpawnUnit(3, Team.Player1, MapManager.ins.currentMapData.start1.ToVector2());
        SpawnUnit(2, Team.Player2, MapManager.ins.currentMapData.start2.ToVector2());
        SpawnUnit(2, Team.Player1, new Vector2(3, 5));
    }
    #endregion
    #region Input Handling
    //
    private void AddLIsteners()
    {
        InputEvents.ins.OnMouseLClick += HandleMouseLClick;
        InputEvents.ins.OnMouseRClick += HandleMouseRClick;
    }
    private void RemoveListeners()
    {
        InputEvents.ins.OnMouseLClick -= HandleMouseLClick;
        InputEvents.ins.OnMouseRClick -= HandleMouseRClick;
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
                    TempMoveUnit();
                }
                break;

            case GameState.awaitingAction:
            
                break;

            case GameState.awaitingSplit:
                if(currentSelected.inSplitRange.Contains(cursor.gridpos))
                {
                    ConfirmSplit(cursor.gridpos);
                    DeselectAll();
                }
                break;

            case GameState.awaitingAttack:
                if(currentSelected.inAttackRange.Contains(cursor.gridpos))
                {
                    ConfirmAttack(cursor.gridpos);
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
                if (MapManager.ins.unitGrid.ContainsKey(cursor.gridpos))
                {
                    var unit = MapManager.ins.unitGrid[cursor.gridpos];
                    unit.CheckAttackRange();
                    foreach(Vector2 v in unit.inAttackRange) MapManager.ins.currentMap[v].SetColor(Color.red / 2);
                }
                break;
            case GameState.unitSelected:
                DeselectAll();
                break;
            case GameState.awaitingAction:
                Undo();
                break;
            case GameState.awaitingAttack:

                break;
            case GameState.paused:
                break;
        }
    }
    #endregion
    #region Unit Control
    //
    private void SpawnUnit(int type, Team team, Vector2 gridPos)
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
    private void ConfirmSplit(Vector2 splitPos)
    {
        SpawnUnit(1, currentSelected.team, splitPos);
        MapManager.ins.unitGrid[splitPos].Sleep();
        currentSelected.TakeDamage(1);
        currentSelected.Sleep();
        ConfirmMove();        
    }
    private void ConfirmAttack(Vector2 attackPos)
    {
        var attacked =MapManager.ins.unitGrid[attackPos];
        attacked.TakeDamage(1);
        if(attacked.state == UnitState.dead) KillUnit(attackPos);
        currentSelected.Sleep();
        ConfirmMove();
    }
    private void KillUnit(Vector2 unitPos)
    {
        MapManager.ins.RemoveUnit(unitPos);
    }
    //
    private void TempMoveUnit()
    {
        // move the unit
        _currentSelected.SetPrevPos(MapManager.ins.WorldToGrid(_currentSelected.transform.position));
        _currentSelected.Move(cursor.gridpos);
        _currentSelected.CheckAttackRange();
        _currentSelected.CheckForEnemies();
        _currentSelected.CheckForFood();
        _currentSelected.CheckSplitRange();
        _currentSelected.AwaitAction();
        DeselectAllTiles();
        

        // setup the action menu based on what is around the unit
        gameUI.ClearActionMenu();
        if (_currentSelected.enemiesInRange.Count > 0) gameUI.AddAttackButton().onClick.AddListener(HandleAttack);
        if (_currentSelected.inEatRange.Count > 0) gameUI.AddEatButton().onClick.AddListener(HandleEat);
        if (_currentSelected.data.tier > 1) gameUI.AddSplitButton().onClick.AddListener(HandleSplit);
        gameUI.AddWaitButton().onClick.AddListener(HandleWait);
        

        // change the game state to wait for an action to be selected
        ChangeState(GameState.awaitingAction);
    }
    private void ConfirmMove()
    {
        MapManager.ins.unitGrid.Remove(_currentSelected.prevPos);
        MapManager.ins.unitGrid.Add(MapManager.ins.WorldToGrid(_currentSelected.transform.position), _currentSelected);
        _currentSelected = null;
        ChangeState(GameState.awaitingInput);
    }
    //
    private void Undo()
    {
        _currentSelected.Undo();
        ChangeState(GameState.awaitingInput);
    }
    #endregion
    #region Action Menu Handlers
    //
    public void HandleWait()
    {
        _currentSelected.Sleep();
        ChangeState(GameState.awaitingInput);
        ConfirmMove();
    }
    //
    public void HandleAttack()
    {
        foreach(Vector2 v in _currentSelected.enemiesInRange)
        {
            MapManager.ins.currentMap[v].SetColor(Color.red / 2);
        }
        ChangeState(GameState.awaitingAttack);
    }
    //
    public void HandleEat()
    {

    }
    //
    public void HandleSplit()
    {
        foreach(Vector2 v in currentSelected.inSplitRange)
        {
            MapManager.ins.currentMap[v].SetColor(Color.yellow);
        }
        ChangeState(GameState.awaitingSplit);
    }
    #endregion
    #region Game Flow
    //
    private void ChangeState(GameState newState)
    {
        _gameState = newState;
        switch (_gameState)
        {
            case GameState.awaitingInput:
                cursor.Show();
                gameUI.actionMenu.Hide();
                break;
            case GameState.awaitingAction:
                cursor.Hide();
                gameUI.ShowActionMenu();
                break;
            case GameState.awaitingSplit:
                cursor.Show();
                gameUI.actionMenu.Hide();
                break;
            case GameState.awaitingAttack:
                cursor.Show();
                gameUI.actionMenu.Hide();
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
        foreach (KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap) { k.Value.UnHighlight(); }
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



