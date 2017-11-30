using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    
    // if this tile is clicked on
    protected bool _isSelected = false;
	// _isSelected
    public bool isSelected { get { return _isSelected; } }
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
    // select the object
    public virtual void Select() { _isSelected = true; }
    // Deselect
    public virtual void Deselect() { _isSelected = false; UnHighlight(); }
    // highlight the tile
    public virtual void Highlight(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
    // remove the highlight from the tile
    public virtual void UnHighlight()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
