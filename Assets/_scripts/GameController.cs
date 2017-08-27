using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // singleton of game controller
    [HideInInspector] public static GameController instance = null; 
    // reference to the selector
    private Selector selector;
    // is there currently tile selected
    private bool _aTileIsSelected = false;
    public bool aTileIsSelected { get { return _aTileIsSelected; } }
    // list of tiles of the current map
    private Dictionary<Vector2, Tile> tiles; 



    // called when instantiated
    private void Awake()
    {
        // set instance to current instance
        GameController.instance = this;
        // get reference to the selector
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>()); 
    }
    // called when scene starts
    private void Start()
    {
        Collections.Load(); 
        LoadMap(2);
    }
    // called once a frame
    private void Update()
    {
        if(!_aTileIsSelected) MoveSelector();
        if(Input.GetMouseButtonDown(0)) { CheckClick(); }
    }
    // load the map
    private void LoadMap(int mapID)
    {
        // spawn the map and load the tiles
        tiles = MapLoader.instance.Load(mapID); 
    }
    // destroy every tile and clear the dictionary
    private void RemoveMap()
    {
        foreach (KeyValuePair<Vector2, Tile> item in tiles) { Destroy(item.Value.gameObject); }
        tiles.Clear();
    }
    // move the selector to currently hovered tile
    private void MoveSelector()
    {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) 
        {
            if(k.Value.isHovered) selector.Move(k.Value.transform.position);
        }
    }
    // check what tile is clicked
    private void CheckClick()
    {
        Tile selectedTile = null;
        Tile hoveredTile = null;

        // for each tile...
        foreach (KeyValuePair<Vector2, Tile> k in tiles) 
        {
            if(k.Value.isSelected) selectedTile = k.Value;
            if(k.Value.isHovered) hoveredTile = k.Value;
            if(!selectedTile && hoveredTile) hoveredTile.isSelected = _aTileIsSelected = true;
            else if (selectedTile != hoveredTile) DeselectAllTiles();
        }
    }
    // deselects all tiles
    private void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) k.Value.isSelected = _aTileIsSelected = false;
    }
}
