using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    // if this tile is clicked on
    protected bool _isSelected = false;
	// _isSelected
    public bool isSelected { get { return _isSelected; } }
    // if the tile is being hovered over   
    protected bool _isHovered = false;
	// _isHovered 
    public bool isHovered { get { return _isHovered; } }
    // dictionary for holding all of the sprites
    protected Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>(); 




    // called when instantiated
    void Awake() { LoadSprites(); }
    // update
    void Update() { }
    // load the sprites from the resources folder
    protected virtual void LoadSprites() { }
    // set the type of tile that should be used, changing its sprite and data
    public virtual void SetType(int type) { }
    //
    public virtual void Select() { _isSelected = true; }
    // Deselect
    public virtual void Deselect() { _isSelected = false; }
    // if mouse is over this tile, it is hovered
    void OnMouseOver() { _isHovered = true; }
    // if mouse moves off this tile, it is no longer hovered
    void OnMouseExit() { _isHovered = false; } 
}
