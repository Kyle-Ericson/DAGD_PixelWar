using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [HideInInspector] public static GameController instance = null; // singleton of game controller
    private Selector selector; // reference to the selector
    private bool _isATileSelected = false;
    public bool isATileSelected { get { return _isATileSelected; } }
    private Dictionary<Vector2, Tile> tiles; // list of tiles of the current map



    // called when instantiated
    private void Awake()
    {
        GameController.instance = this;
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>()); // get reference to the selector
    }
    // called when scene starts
    private void Start()
    {
        Collections.Load(); // load all the libraries
        LoadMap(2); // load the map
    }
    // called once a frame
    private void Update()
    {
        if(!_isATileSelected) MoveSelector();
        if(Input.GetMouseButtonDown(0)) { CheckClick(); }
    }
    // load the map
    private void LoadMap(int mapID)
    {
        tiles = MapLoader.instance.Load(mapID); // spawn the map and load the tiles
    }
    // remove the map
    private void RemoveMap()
    {
        foreach (KeyValuePair<Vector2, Tile> item in tiles) { Destroy(item.Value.gameObject); }  // destroy the tiles
        tiles.Clear(); // clear tiles dictionary
    }
    private void MoveSelector()
    {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) 
        {
            if(k.Value.isHovered) selector.Move(k.Value.transform.position);
        }
    }
    private void CheckClick()
    {
        Tile selectedTile = null;
        Tile hoveredTile = null;

        foreach (KeyValuePair<Vector2, Tile> k in tiles) // for each tile...
        {
            if(k.Value.isSelected) selectedTile = k.Value;
            if(k.Value.isHovered) hoveredTile = k.Value;

            if(!selectedTile && hoveredTile) hoveredTile.isSelected = _isATileSelected = true;
            else if (selectedTile != hoveredTile) DeselectAllTiles();
        }
    }
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) k.Value.isSelected = _isATileSelected = false;
    }
}
