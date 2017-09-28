using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece 
{

    public UnitItem data = null;



    // load Unit sprites from resources
    protected override void LoadSprites() {
        
    }
    // set type of unit
    public override void SetType(int type) {
        GetComponent<SpriteRenderer>().sprite = sprites[type];
	    data = Collections.units[type];	
    }
}
