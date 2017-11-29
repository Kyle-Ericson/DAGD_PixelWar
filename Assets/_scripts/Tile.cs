using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// the tile object
public class Tile : GamePiece 
{
    private TileData _data = null;
    public TileData data { get { return _data; }}
    // load the sprites from the resources folder
    protected override void LoadSprites() {
        sprites.Add(1, (Resources.Load<Sprite>("sprites/prototype/mountain")));
        sprites.Add(2, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    // set the type of tile that should be used, changing its sprite and data
    public override void SetType(int type) {
        GetComponent<SpriteRenderer>().sprite = sprites[type]; 
        _data = Collections.tileCollection.tileData[type]; 
    }
    
}
