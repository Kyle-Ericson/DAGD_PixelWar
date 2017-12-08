using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // reference to itself
    [HideInInspector] public static GameController instance = null; 
    // reference to the selector
    private Selector selector;
    // unit prefab
    private GameObject unitObj;
    // unt z offset
    private float zoffset = -0.1f;
    // list of tiles of the current map
    private Dictionary<Vector2, Tile> _grid = null;
    public Dictionary<Vector2, Tile> grid { get { return _grid; } }
    // list of units on the board
    private Dictionary<Vector2, Unit> units = new Dictionary<Vector2, Unit>();
    // reference to the maploader instance
    private MapLoader maploader = null;




    // called when instantiated
    private void Awake() {
        // set instance to current instance
        if(instance == null) instance = this;
        if(maploader == null) maploader = MapLoader.instance;
        // spawn selector
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>());
        selector.Hide(); 
        // load unit object
        unitObj = Resources.Load<GameObject>("prefabs/Unit");
    }
    // called when scene starts
    private void Start() {
        Collections.Load(); 
        instance.LoadMap(0);
        selector.Move(maploader.GridToWorld(Vector2.zero));
        selector.Show();
    }
    // called once a frame
    private void Update() {
        instance.MoveSelector();
        instance.CheckInput();
    }
    // check input
    private void CheckInput() {
        if(Input.GetMouseButtonDown(0)) {
            Vector2 gPos = maploader.WorldToGrid(selector.transform.position);
            DeselectAll();
            if(units.ContainsKey(gPos)) {
                units[gPos].Select();
               
            }
        }
    }
    // spawn the map and load the tiles
    private void LoadMap(int mapID) {
        _grid = maploader.LoadMap(mapID);
        SpawnUnit(UnitType.tank, maploader.currentMap.start1.ToVector2());
    }
    // spawn units
    private void SpawnUnit(UnitType type, Vector2 gridPos) {
        Vector3 worldPos = maploader.GridToWorld(gridPos);
        worldPos.z = zoffset;
        GameObject newUnit = Instantiate(unitObj, worldPos, Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        unit.SetType((int)type);
        Debug.Log("New " + unit.data.name + " has spawned at " + gridPos + "!");
        units.Add(gridPos, unit);
    }
    // destroy every tile and clear the dictionary
    private void RemoveMap() {
        foreach (KeyValuePair<Vector2, Tile> item in _grid) { Destroy(item.Value.gameObject); }
        _grid.Clear();
    }
    // move the selector
    private void MoveSelector() {
        Vector2 gridPos = maploader.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(grid.ContainsKey(gridPos)) {
            selector.Move(maploader.GridToWorld(gridPos));
        }
    }
    // deselects all tiles
    private void DeselectAll() {
        DeselectAllTiles();
        DeselectAllUnits();
    }
    private void DeselectAllTiles() {
        foreach (KeyValuePair<Vector2, Tile> k in grid) k.Value.Deselect(); 
    }
    private void DeselectAllUnits() {
        foreach (KeyValuePair<Vector2, Unit> k in units) k.Value.Deselect(); 
    }
    private void RefreshUnitList()
    {
        // TODO: Refresh the unit list here making sure all units are in the correct positions
        Dictionary<Vector2, Unit> unitsCopy = units;
        units.Clear();
    }
    
}
