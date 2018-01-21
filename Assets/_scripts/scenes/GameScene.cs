using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class GameScene : ESingletonMono<GameScene>
{
    private GameObject unitPrefab;
    private Cursor cursor;
    private Unit currentSelected = null;
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
                if (currentSelected.tilesWithinMoveRange.Contains(cursor.gridpos)) TempMoveUnit();
                break;
            case GameState.awaitingAction:
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
                    MapManager.ins.unitGrid[cursor.gridpos].CheckAttackRange();
                }
                break;
            case GameState.unitSelected:
                DeselectAll();
                break;
            case GameState.awaitingAction:
                Undo();
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
        currentSelected = MapManager.ins.unitGrid[cursor.gridpos];
        currentSelected.Select();
        currentSelected.CheckMove();
        ChangeState(GameState.unitSelected);
    }
    //
    private void TempMoveUnit()
    {
        // move the unit
        currentSelected.SetPrevPos(MapManager.ins.WorldToGrid(currentSelected.transform.position));
        currentSelected.Move(cursor.gridpos);
        currentSelected.AwaitAction();
        DeselectAllTiles();
        

        // setup the action menu based on what is around the unit
        gameUI.ClearActionMenu();
        if (currentSelected.enemiesWithinRange.Count > 0) gameUI.AddAttackButton();
        if (currentSelected.foodWithinRange.Count > 0) gameUI.AddEatButton();
        if (currentSelected.data.tier > 1) gameUI.AddSplitButton();
        gameUI.AddWaitButton().onClick.AddListener(HandleWait);
        

        // change the game state to wait for an action to be selected
        ChangeState(GameState.awaitingAction);
    }
    private void ConfirmMove()
    {
        MapManager.ins.unitGrid.Remove(currentSelected.prevPos);
        MapManager.ins.unitGrid.Add(MapManager.ins.WorldToGrid(currentSelected.transform.position), currentSelected);
        currentSelected = null;
        ChangeState(GameState.awaitingInput);
    }
    //
    private void Undo()
    {
        currentSelected.Undo();
        ChangeState(GameState.awaitingInput);
    }
    #endregion
    #region Action Menu Handlers
    //
    public void HandleWait()
    {
        currentSelected.Sleep();
        ChangeState(GameState.awaitingInput);
        ConfirmMove();
    }
    //
    public void HandleAttack()
    {

    }
    //
    public void HandleEat()
    {

    }
    //
    public void HandleSplit()
    {

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
        currentSelected = null;
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



