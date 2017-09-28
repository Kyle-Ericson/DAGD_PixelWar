using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// the tile object
public class Tile : GamePiece 
{
    private bool isHighlighted = false;
    public TileItem data = null;
    
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
    // highlight the tile
    public void Highlight() {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    // remove the highlight from the tile
    public void UnHighlight() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
