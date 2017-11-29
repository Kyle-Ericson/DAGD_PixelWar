using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece 
{

    private UnitData _data = null;
    public UnitData data { get { return _data; }}


    // load Unit sprites from resources
    protected override void LoadSprites() {
        //sprites.Add(0, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    // set type of unit
    public override void SetType(int type) {
        //GetComponent<SpriteRenderer>().sprite = sprites[type];
	    _data = Collections.unitCollection.unitData[type];	
    }
    public override void Select() {
        Highlight(Color.red);
        base.Select();
    }
    public override void Deselect() {
        UnHighlight();
        base.Select();
    }
}
