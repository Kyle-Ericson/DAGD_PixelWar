using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [HideInInspector] public static GameController instance = null; // singleton of game controller
    private Selector selector; // reference to the selector
    private bool _isATileSelected = false;
    public bool isATileSelected
    {
        get { return _isATileSelected; }
        set { _isATileSelected = value; }
    }
    private Dictionary<Vector2, Tile> tiles; // list of tiles of the current map




    private void Awake() // called when instantiated
    {
        GameController.instance = this; // singleton gamecontroller instance
        selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>()); // get reference to the selector
    }
    private void Start() // called when scene starts
    {
        Collections.Load(); // load all the libraries
        LoadMap(2);
    }
    private void Update() // called once a frame
    {
        CheckTiles();
    }
    private void LoadMap(int mapID) // load the map
    {
        tiles = MapLoader.instance.Load(mapID); // spawn the map and load the tiles
    }
    private void RemoveMap() // remove the map
    {
        foreach (KeyValuePair<Vector2, Tile> item in tiles) { Destroy(item.Value.gameObject); }  // destroy the tiles
        tiles.Clear(); // clear tiles dictionary
    }
    private void CheckTiles() // move the selector to the correct tile
    {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) // for each tile...
        {
            if (k.Value && k.Value.isHovered && !_isATileSelected) // if it is selected...
            {
                selector.Move(k.Value.transform.position); // move the selector
            }
        }
    }
}
