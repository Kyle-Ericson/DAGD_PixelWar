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

    private PlayState currentPlayState = PlayState.awaitingInput;




    // called when instantiated
    private void Awake() {
        if(instance == null) instance = this;
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>());
        unitObj = Resources.Load<GameObject>("prefabs/Unit");
    }
    // called when scene starts
    private void Start() {
        Collections.Load(); 
        instance.LoadMap(0);
        selector.Move(MapLoader.instance.GridToWorld(Vector2.zero));
        selector.Show();
    }
    // called once a frame
    private void Update() {
        instance.MoveSelector();
        instance.CheckInput();
    }
    // check input
    private void CheckInput() {
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentPlayState)
            {
                case PlayState.awaitingInput:
                    if (units.ContainsKey(selector.gridPosition))
                    {
                        units[selector.gridPosition].Select();
                    }
                    else DeselectAll();

                    break;
                case PlayState.unitSelected:

                    break;
            }
        }
        
    }
    // spawn the map and load the tiles
    private void LoadMap(int mapID) {
        _grid = MapLoader.instance.LoadMap(mapID);
        SpawnUnit(UnitType.tank, MapLoader.instance.currentMap.start1.ToVector2());
    }
    // spawn units
    private void SpawnUnit(UnitType type, Vector2 gridPos) {
        Vector3 worldPos = MapLoader.instance.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitObj, worldPos, Quaternion.identity).GetComponent<Unit>();
        unit.SetType((int)type);
        units.Add(gridPos, unit);
    }
    // destroy every tile and clear the dictionary
    private void RemoveMap() {
        foreach (KeyValuePair<Vector2, Tile> item in _grid) { Destroy(item.Value.gameObject); }
        _grid.Clear();
    }
    // move the selector
    private void MoveSelector() {
        Vector2 gridPos = MapLoader.instance.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(grid.ContainsKey(gridPos)) {
            selector.Move(MapLoader.instance.GridToWorld(gridPos));
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
    
}



