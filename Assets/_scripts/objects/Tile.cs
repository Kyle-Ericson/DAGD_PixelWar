using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class Tile : MonoBehaviour // the tiles of the map
{

    private bool _isSelected = false; // if this tile is currently selected
    public bool isSelected { get { return _isSelected; } }
    private TileData data;
    private Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>(); // dictionary for holding all of the sprites


    void Awake()
    {
        LoadSprites();
    }
    void LoadSprites()
    {
        sprites.Add(1, (Resources.Load<Sprite>("sprites/prototype/mountain"))); // add mountain sprite to sprites
        sprites.Add(2, (Resources.Load<Sprite>("sprites/prototype/forest"))); // add forest sprite to sprites
    }
    public void SetType(int type) // set the type of tile that should be used
    {
        GetComponent<SpriteRenderer>().sprite = sprites[type];
        data = Collections.tiles[type];
        Debug.Log(data.name);
    }
    void OnMouseOver() {
        _isSelected = true; // if mouse is over this tile, it is selected
    }
    void OnMouseExit() {
        _isSelected = false; // if mouse moves off this tile, it is no longer selected
    }
}
