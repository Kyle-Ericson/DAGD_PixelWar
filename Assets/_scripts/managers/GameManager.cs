using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject unitPrefab;    
    private Selector selector;
    private float zoffset = -0.1f;    
    private Dictionary<Vector2, Unit> units = new Dictionary<Vector2, Unit>();
    private GameState currentPlayState = GameState.awaitingInput;    
    public MapManager mm;




    private GameManager() { }

    //
    private void Awake()
    {
        mm = MapManager.ins;
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>());
        unitPrefab = Resources.Load<GameObject>("prefabs/Unit");
    }
    //
    private void Start()
    {
        Database.Load();
        LoadMap(0);
        selector.Move(mm.GridToWorld(Vector2.zero));
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
            switch (currentPlayState)
            {
            case GameState.awaitingInput:
                if (units.ContainsKey(mm.WorldToGrid(selector.transform.position)))
                {
                    units[mm.WorldToGrid(selector.transform.position)].Select();
                    units[mm.WorldToGrid(selector.transform.position)].CheckMoveDistance();
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
        mm.SpawnMap(mapID);
        SpawnUnit(UnitType.tank, mm.currentMapData.start1.ToVector2());
    }
    //
    private void SpawnUnit(UnitType type, Vector2 gridPos)
    {
        Vector3 worldPos = mm.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitPrefab, worldPos, Quaternion.identity).GetComponent<Unit>();
        unit.SetType((int)type);
        units.Add(gridPos, unit);
    }
    //
    private void RemoveMap()
    {
        foreach (KeyValuePair<Vector2, Tile> item in mm.currentMap) { Destroy(item.Value.gameObject); }
        mm.currentMap.Clear();
    }
    //
    private void MoveSelector()
    {
        Vector2 gridPos = mm.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (mm.currentMap.ContainsKey(gridPos))
        {
            selector.Move(mm.GridToWorld(gridPos));
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
        foreach (KeyValuePair<Vector2, Tile> k in mm.currentMap) { k.Value.Deselect(); }
    }
    //
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in units) { k.Value.Deselect(); }
    }
}
