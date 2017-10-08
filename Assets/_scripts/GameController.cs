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
    private Dictionary<Vector2, Tile> tiles = null;
    // list of units on the board
    private Dictionary<Vector2, Unit> units = new Dictionary<Vector2, Unit>();
    // tile width and height
    float tileWidth = 1f;
    float tileHeight = 1f;




    // called when instantiated
    private void Awake() {
        // set instance to current instance
        if(GameController.instance == null) GameController.instance = this;
        // get reference to the selector
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>()); 
        // load unit object
        unitObj = Resources.Load<GameObject>("prefabs/Unit");
    }
    // called when scene starts
    private void Start() {
        Collections.Load(); 
        GameController.instance.LoadMap(0);
    }
    // called once a frame
    private void Update() {
        GameController.instance.MoveSelector();
        GameController.instance.CheckInput();
    }
    // check input
    private void CheckInput() {
        if(Input.GetMouseButtonDown(0)) {
            Vector2 gPos = WorldToGrid(selector.transform.position);
            DeselectAll();
            if(units.ContainsKey(gPos)) {
                units[gPos].Select();
                CheckNeighbors(units[gPos]);
            }
        }
    }
    // spawn the map and load the tiles
    private void LoadMap(int mapID) {
        tiles = MapLoader.instance.LoadMap(mapID);
        Vector2 start1 = MapLoader.currentMap.start1.ToVector2();
        SpawnUnit(0, start1);

    }
    // spawn units
    private void SpawnUnit(int type, Vector2 gridPos) {
        Vector3 worldPos = GridToWorld(gridPos);
        worldPos.z = zoffset;
        GameObject newUnit = Instantiate(unitObj, worldPos, Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        unit.SetType(type);
        Debug.Log("New " + unit.data.name + " has spawned at " + gridPos + "!");
        units.Add(gridPos, unit);
    }
    // destroy every tile and clear the dictionary
    private void RemoveMap() {
        foreach (KeyValuePair<Vector2, Tile> item in tiles) { Destroy(item.Value.gameObject); }
        tiles.Clear();
    }
    // move the selector
    private void MoveSelector() {
        Vector2 gridPos = WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(tiles.ContainsKey(gridPos)) {
            selector.transform.position = tiles[gridPos].transform.position;
        }
    }
    // check this units neighbors based on its range
    private void CheckNeighbors(Unit unit) {
        Vector2 gridPos = WorldToGrid(unit.transform.position);
        Debug.Log(gridPos);
    }
    // deselects all tiles
    private void DeselectAll() {
        DeselectAllTiles();
        DeselectAllUnits();
    }
    private void DeselectAllTiles() {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) k.Value.Deselect(); 
    }
    private void DeselectAllUnits() {
        foreach (KeyValuePair<Vector2, Unit> k in units) k.Value.Deselect(); 
    }
    // convert a world position into a grid position
    private Vector2 WorldToGrid(Vector2 worldPos) {
        return new Vector2(Mathf.Floor(worldPos.x / tileWidth), -Mathf.Floor(worldPos.y / tileHeight) - 1);
    }
    private Vector3 GridToWorld(Vector2 gridPos) {
        return new Vector3(gridPos.x * tileWidth + (tileWidth / 2), -gridPos.y * tileHeight - (tileHeight / 2), 0);
    }
}
