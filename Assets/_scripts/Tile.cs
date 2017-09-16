using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// the tile object
public class Tile : GamePiece 
{
    private TileItem data;
    private bool isHighlighted = false;
    

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(isHovered) _isSelected = true;
            else _isSelected = false;
        }
        ChangeColor();
    }
    // change the sprites color based on selected status
    public void ChangeColor() {
        if (isSelected) GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.white;
    }
    // load the sprites from the resources folder
    protected override void LoadSprites() {
        sprites.Add(1, (Resources.Load<Sprite>("sprites/prototype/mountain")));
        sprites.Add(2, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    // set the type of tile that should be used, changing its sprite and data
    public override void SetType(int type) {
        GetComponent<SpriteRenderer>().sprite = sprites[type]; 
        data = Collections.tiles[type]; 
    }
}
