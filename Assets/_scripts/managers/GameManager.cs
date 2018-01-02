using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject unitPrefab;    
    private Selector selector;
    private float zoffset = -0.1f;    
    private Dictionary<Vector2, Unit> units = new Dictionary<Vector2, Unit>();
    private GameState gameState = GameState.awaitingInput;


    //
    private void Awake()
    {
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>());
        unitPrefab = Resources.Load<GameObject>("prefabs/Unit");
    }
    //
    private void Start()
    {
        Database.Load();
        LoadMap(0);
        selector.Move(MapManager.ins.GridToWorld(Vector2.zero));
        selector.Show();
    }
    //
    private void Update()
    {
        MoveSelector();
        CheckInput();
    }
    //
    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (gameState)
            {
            case GameState.awaitingInput:
                if (units.ContainsKey(MapManager.ins.WorldToGrid(selector.transform.position)))
                {
                    units[MapManager.ins.WorldToGrid(selector.transform.position)].Select();
                    units[MapManager.ins.WorldToGrid(selector.transform.position)].CheckMoveDistance();
                }
                else { DeselectAll(); }

                break;

            case GameState.unitSelected:

                break;
            }
        }
    }
    //
    private void LoadMap(int mapID)
    {
        MapManager.ins.SpawnMap(mapID);
        SpawnUnit(UnitType.tank, MapManager.ins.currentMapData.start1.ToVector2());
    }
    //
    private void SpawnUnit(UnitType type, Vector2 gridPos)
    {
        Vector3 worldPos = MapManager.ins.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitPrefab, worldPos, Quaternion.identity).GetComponent<Unit>();
        unit.SetType((int)type);
        units.Add(gridPos, unit);
    }
    //
    private void RemoveMap()
    {
        foreach (KeyValuePair<Vector2, Tile> item in MapManager.ins.currentMap) { Destroy(item.Value.gameObject); }
        MapManager.ins.currentMap.Clear();
    }
    //
    private void MoveSelector()
    {
        Vector2 gridPos = MapManager.ins.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (MapManager.ins.currentMap.ContainsKey(gridPos))
        {
            selector.Move(MapManager.ins.GridToWorld(gridPos));
        }
    }
    //
    private void DeselectAll()
    {
        DeselectAllTiles();
        DeselectAllUnits();
    }
    //
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap) { k.Value.Deselect(); }
    }
    //
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in units) { k.Value.Deselect(); }
    }
}
