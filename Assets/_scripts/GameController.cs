using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // reference to itself
    [HideInInspector] public static GameController instance = null; 
    // reference to the selector
    private Selector selector;
    // is there currently tile selected
    private bool _aTileIsSelected = false;
    public bool aTileIsSelected { get { return _aTileIsSelected; } }
    // list of tiles of the current map
    private Dictionary<Vector2, Tile> tiles = null;



    // called when instantiated
    private void Awake() {
        // set instance to current instance
        GameController.instance = this;
        // get reference to the selector
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>()); 
    }
    // called when scene starts
    private void Start() {
        Collections.Load(); 
        LoadMap(0);
    }
    // called once a frame
    private void Update() {
        if(!_aTileIsSelected) MoveSelector();
        CheckSelected();
    }
    // spawn the map and load the tiles
    private void LoadMap(int mapID) {
        tiles = MapLoader.Load(mapID);
    }
    // destroy every tile and clear the dictionary
    private void RemoveMap() {
        foreach (KeyValuePair<Vector2, Tile> item in tiles) { Destroy(item.Value.gameObject); }
        tiles.Clear();
    }
    // move the selector to currently hovered tile
    private void MoveSelector() {
        if(tiles != null) {
            foreach (KeyValuePair<Vector2, Tile> k in tiles) 
            {
                if(k.Value.isHovered) selector.Move(k.Value.transform.position);
            }
        }
    }
    // check what tile is clicked
    private void CheckSelected() {
        Tile selectedTile = null;

        foreach (KeyValuePair<Vector2, Tile> k in tiles) { 
            if(k.Value.isSelected) {
                _aTileIsSelected = true;
                selectedTile = k.Value;
            } 
            else {
                _aTileIsSelected = false;
            }
        }

    }
    // deselects all tiles
    private void DeselectAllTiles() {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) { 
            k.Value.Deselect(); 
        }
        _aTileIsSelected = false;
    }
}
