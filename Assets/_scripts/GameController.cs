using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance = null;
    public static GameController instance
    {
        get { return _instance; }
    }
    private Dictionary<Vector2, Tile> _currentMap = null;
    public Dictionary<Vector2, Tile> currentMap
    {
        get { return _currentMap; }
    }
    private Selector selector;
    private GameObject unitObj;
    private float zoffset = -0.1f;    
    private Dictionary<Vector2, Unit> units = new Dictionary<Vector2, Unit>();
    private GameState currentPlayState = GameState.awaitingInput;
    private MapLoader mapLoader = null;




    private GameController() { }

    private void Awake()
    {
        if (_instance == null) { _instance = this; }
        mapLoader = MapLoader.instance;
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>());
        unitObj = Resources.Load<GameObject>("prefabs/Unit");
    }
    private void Start()
    {
        Collections.Load();
        LoadMap(0);
        selector.Move(mapLoader.GridToWorld(Vector2.zero));
        selector.Show();
    }
    private void Update()
    {
        MoveSelector();
        CheckInput();
    }
    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentPlayState)
            {
            case GameState.awaitingInput:
                if (units.ContainsKey(selector.gridPosition))
                {
                    units[selector.gridPosition].Select();
                }
                else{ DeselectAll(); }

                break;

            case GameState.unitSelected:

                break;
            }
        }
    }
    private void LoadMap(int mapID)
    {
        _currentMap = mapLoader.LoadMap(mapID);
        SpawnUnit(UnitType.tank, mapLoader.currentMap.start1.ToVector2());
    }
    private void SpawnUnit(UnitType type, Vector2 gridPos)
    {
        Vector3 worldPos = mapLoader.GridToWorld(gridPos);

        worldPos.z = zoffset;
        Unit unit = Instantiate(unitObj, worldPos, Quaternion.identity).GetComponent<Unit>();
        unit.SetType((int)type);
        units.Add(gridPos, unit);
    }
    private void RemoveMap()
    {
        foreach (KeyValuePair<Vector2, Tile> item in _currentMap) { Destroy(item.Value.gameObject); }
        _currentMap.Clear();
    }
    private void MoveSelector()
    {
        Vector2 gridPos = mapLoader.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (_currentMap.ContainsKey(gridPos))
        {
            selector.Move(mapLoader.GridToWorld(gridPos));
        }
    }
    private void DeselectAll()
    {
        DeselectAllTiles();
        DeselectAllUnits();
    }
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in _currentMap) { k.Value.Deselect(); }
    }
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in units) { k.Value.Deselect(); }
    }
}
