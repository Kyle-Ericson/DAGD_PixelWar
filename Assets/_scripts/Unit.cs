using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece 
{

    // unit data
    private UnitItem data;

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(isHovered) _isSelected = true;
        }
    }
    // load Unit sprites from resources
    protected override void LoadSprites() {
        
    }
    public override void SetType(int type) {
        GetComponent<SpriteRenderer>().sprite = sprites[type];
	    data = Collections.units[type];	
    }
    

}
