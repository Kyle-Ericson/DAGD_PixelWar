using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// the tile object
public class Tile : MonoBehaviour 
{
    // if this tile is clicked on
    private bool _isSelected = false;
    // if the tile is being hovered over   
    private bool _isHovered = false; 
    // the data for this tile's type
    private TileItem data;
    // dictionary for holding all of the sprites
    private Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>(); 
    


    // _isHovered 
    public bool isHovered 
    {   
        get { return _isHovered; } 
    }
    // _isSelected
    public bool isSelected 
    { 
        get { return _isSelected; } 
        set { _isSelected = value; ChangeColor(); }
    }
    // called when instantiated
    void Awake()
    {
        LoadSprites();
    }
    // change the sprites color based on selected status
    void ChangeColor() 
    {
        if (_isSelected) GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.white;
    }
    // load the sprites from the resources folder
    void LoadSprites() 
    {
        sprites.Add(1, (Resources.Load<Sprite>("sprites/prototype/mountain")));
        sprites.Add(2, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    // set the type of tile that should be used, changing its sprite and data
    public void SetType(int type) 
    {
        GetComponent<SpriteRenderer>().sprite = sprites[type]; 
        data = Collections.tileData[type]; 
    }
    // if mouse is over this tile, it is hovered
    void OnMouseOver() { _isHovered = true; }
    // if mouse moves off this tile, it is no longer hovered
    void OnMouseExit() { _isHovered = false; } 
}
