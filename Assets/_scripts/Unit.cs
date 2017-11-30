using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece 
{

    private UnitData _data = null;
    public UnitData data { get { return _data; }}
    public int actionPoints = 3;


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
        base.Select();
        Highlight(Color.red);
        CheckMoveDistance();
    }
    public override void Deselect() {
        base.Deselect();
    }
    public void CheckMoveDistance()
    {
       for(int i = -data.speed; i <= data.speed; i++)
        {
            for(int j = - data.speed; j <= data.speed; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > data.speed || i == 0 && j == 0) continue;
                var gPos = MapLoader.instance.WorldToGrid(transform.position);
                gPos.x += i;
                gPos.y += j;
                var tile = GameController.instance.grid[gPos];
                if (tile.data.isWalkable)
                {
                    tile.Highlight(Color.red);
                }
            }
            
        }
    }
}
