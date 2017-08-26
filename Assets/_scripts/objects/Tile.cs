using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Tile : MonoBehaviour // the tiles of the map
{

    private bool _isSelected = false; // if this tile is clicked on
    private bool _isHovered = false; // if the tile is being hovered over   
    private TileItem data; // the data for this tile's type
    private Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>(); // dictionary for holding all of the sprites
    




    public bool isHovered 
    {   
        get { return _isHovered; } 
    }
    public bool isSelected 
    { 
        get { return _isSelected; } 
        set { _isSelected = value; ChangeColor(); }
    }




    void Awake()
    {
        LoadSprites(); // load in the correct sprites
    }
    void ChangeColor() // change the sprites color based on selected status
    {
        if (_isSelected) GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.white;
    }
    void LoadSprites()
    {
        sprites.Add(1, (Resources.Load<Sprite>("sprites/prototype/mountain"))); // add mountain sprite to sprites
        sprites.Add(2, (Resources.Load<Sprite>("sprites/prototype/forest"))); // add forest sprite to sprites
    }
    public void SetType(int type) // set the type of tile that should be used
    {
        GetComponent<SpriteRenderer>().sprite = sprites[type]; // render the correct sprite
        data = Collections.tileData[type]; // get the date for this type of tile
    }
    void OnMouseOver() { _isHovered = true; } // if mouse is over this tile, it is selected
    void OnMouseExit() { _isHovered = false; } // if mouse moves off this tile, it is no longer selected
}
