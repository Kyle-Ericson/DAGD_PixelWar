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
    private bool aTileIsSelected = false;
    private Tile currentSelectedTile = null;
    // list of tiles of the current map
    private Dictionary<Vector2, Tile> tiles = null;
    // list of units on the board
    private Dictionary<Vector2, Unit> units = null;



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

        if(Input.GetMouseButtonDown(0)) CheckTouch();
        if(!aTileIsSelected) MoveSelector();
    }
    private void CheckTouch() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 50f, Color.red, 1000000);

        if(Physics2D.Raycast(ray.origin, ray.origin + ray.direction * 50f))
        {
            Debug.Log("Hit");
        }
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
    // deselects all tiles
    private void DeselectAllTiles() {
        foreach (KeyValuePair<Vector2, Tile> k in tiles) { 
            k.Value.UnHighlight(); 
        }
        aTileIsSelected = false;
    }
}
