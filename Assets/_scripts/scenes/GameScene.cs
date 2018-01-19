using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class GameScene : ESingletonMono<GameScene>
{
    private GameObject unitPrefab;
    private Cursor cursor;
    private Unit currentSelected = null;
    private GameState gameState = GameState.awaitingInput;
    private GameUI gameUI = null;
    private Vector2 tempGridPos;
    private Dictionary<Vector2, Unit> unitGrid = new Dictionary<Vector2, Unit>();

    private float zoffset = -0.2f;
    private int numOfPlayers = 0;
    private int currentTurn = 1;
    private int turnCount = 1;


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
        switch (gameState)
        {
            case GameState.awaitingInput:
                if (unitGrid.ContainsKey(cursor.gridpos) && unitGrid[cursor.gridpos].state != UnitState.sleeping)
                {
                    if(unitGrid[cursor.gridpos].team == (Team)currentTurn) SelectUnit();
                }
                else DeselectAll();
                break;
            case GameState.unitSelected:
                if (currentSelected.tilesInRange.Contains(cursor.gridpos)) PreviewMoveUnit();
                break;
            case GameState.awaitingAction:
                
                break;
        }
    }
    //
    private void HandleMouseRClick()
    {
        switch(gameState)
        {
            case GameState.awaitingInput:
                if (unitGrid.ContainsKey(cursor.gridpos))
                {
                    unitGrid[cursor.gridpos].CheckAttackRange();
                }
                break;
            case GameState.unitSelected:
                DeselectAll();
                break;
            case GameState.awaitingAction:
                Undo();
                break;
        }
    }
    //
    private void SelectUnit()
    {
        currentSelected = unitGrid[cursor.gridpos];
        currentSelected.Select();
        currentSelected.CheckMove();

        ChangeState(GameState.unitSelected);
    }
    //
    private void PreviewMoveUnit()
    {
        gameUI.ClearActionMenu();
        gameUI.AddButton("Wait").onClick.AddListener(HandleWait);
        gameUI.ShowActionMenu();
        currentSelected.AwaitAction();
        DeselectAllTiles();
        currentSelected.SetPrevPos(MapManager.ins.WorldToGrid(currentSelected.transform.position));
        currentSelected.Move(cursor.gridpos);
        cursor.Hide();
        ChangeState(GameState.awaitingAction);
    }
    private void ConfirmMove()
    {
        unitGrid.Remove(currentSelected.prevPos);
        unitGrid.Add(MapManager.ins.WorldToGrid(currentSelected.transform.position), currentSelected);
        cursor.Show();
        gameUI.actionMenu.Hide();
        currentSelected = null;
        ChangeState(GameState.awaitingInput);
    }
    //
    private void Undo()
    {
        currentSelected.Undo();
        ChangeState(GameState.awaitingInput);
    }
    //
    private void LoadMap(int mapID)
    {
        MapManager.ins.SpawnMap(mapID);
        SpawnUnit(1, Team.Player1, MapManager.ins.currentMapData.start1.ToVector2());
        SpawnUnit(2, Team.Player2, MapManager.ins.currentMapData.start2.ToVector2());
    }
    //
    private void SpawnUnit(int type, Team team, Vector2 gridPos)
    {
        Vector3 worldPos = MapManager.ins.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitPrefab).GetComponent<Unit>();
        unit.gameObject.transform.position = worldPos;
        unit.Init(type, team);
        unitGrid.Add(gridPos, unit);
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
        foreach (KeyValuePair<Vector2, Unit> k in unitGrid)
        {
            if(k.Value.state != UnitState.sleeping) k.Value.Deselect();
        }
        currentSelected = null;
    }
    //
    private void ClearScene()
    {
        RemoveAllTiles();
        RemoveAllUnits();
        RemoveListeners();
        cursor.Hide();
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
        foreach (KeyValuePair<Vector2, Unit> item in unitGrid) { Destroy(item.Value.gameObject); }
    }
    //
    public void HandleWait()
    {
        currentSelected.Sleep();
        ChangeState(GameState.awaitingInput);
        ConfirmMove();
    }
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
        Debug.Log("Current Player: " + (Team)currentTurn);
        Debug.Log("Turn: " + turnCount);
    }
    public void WakeUpAll()
    {
        foreach (KeyValuePair<Vector2, Unit> item in unitGrid)
        {
            item.Value.Idle();
        }
    }
    private void ChangeState(GameState newState)
    {
        gameState = newState;
    }
}



