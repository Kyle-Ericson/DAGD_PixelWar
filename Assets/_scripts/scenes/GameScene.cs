using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class GameScene : ESingletonMono<GameScene>
{
    private GameObject unitPrefab;
    private Cursor cursor;
    private Unit currentSelected = null;
    private Vector2 tempGridPos;
    private float zoffset = -0.2f;    
    private Dictionary<Vector2, Unit> unitGrid = new Dictionary<Vector2, Unit>();
    private GameState gameState = GameState.awaitingInput;

    
    //
    private void Start()
    {
        cursor = Instantiate(Resources.Load<GameObject>("prefabs/Cursor").GetComponent<Cursor>());
        unitPrefab = Resources.Load<GameObject>("prefabs/Unit");
        MapManager.ins.gameObject.transform.SetParent(this.gameObject.transform);
        InputEvents.ins.gameObject.transform.SetParent(this.gameObject.transform);
        Sprites.ins.LoadSprites();
        Database.Load();
        SetupMatch(1);
    }
    private void SetupMatch(int map)
    {
        LoadMap(map);
        cursor.Show();
        AddLIsteners();
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
                if (unitGrid.ContainsKey(cursor.gridpos) && unitGrid[cursor.gridpos].state != UnitState.sleeping) SelectUnit();
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
        gameState = GameState.unitSelected;
    }
    //
    private void PreviewMoveUnit()
    {
        currentSelected.AddButton("Wait").onClick.AddListener(HandleWait);
        currentSelected.AwaitAction();
        DeselectAllTiles();
        currentSelected.SetPrevPos(MapManager.ins.WorldToGrid(currentSelected.transform.position));
        currentSelected.Move(cursor.gridpos);
        cursor.Hide();
        gameState = GameState.awaitingAction;
    }
    private void ConfirmMove()
    {
        unitGrid.Remove(currentSelected.prevPos);
        unitGrid.Add(cursor.gridpos, currentSelected);
        cursor.Show();
    }
    //
    private void Undo()
    {
        currentSelected.Undo();
        gameState = GameState.awaitingInput;
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
        gameState = GameState.awaitingInput;
    }
    //
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap) { k.Value.UnHighlight(); }
    }
    //
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in unitGrid) { k.Value.Deselect(); }
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
        gameState = GameState.awaitingInput;
        ConfirmMove();
    }
}



