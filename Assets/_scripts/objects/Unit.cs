using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece 
{

    private UnitData _data = null;
    public UnitData data { get { return _data; }}
    public int actionPoints = 3;
    public List<Tile> tilesInRange = new List<Tile>();
    private GameController gc = null;
    

    public Unit()
    {
        gc = GameController.instance;
    }

    protected override void LoadSprites() {
        //sprites.Add(0, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    public override void SetType(int type) {
        //GetComponent<SpriteRenderer>().sprite = sprites[type];
	    _data = Collections.unitCollection.unitData[type];	
    }
    public override void Select() {
        base.Select();
        CheckMoveDistance();
    }
    public override void UnHighlight()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
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
                if (!gc.currentMap.ContainsKey(gPos)) continue;
                var tile = gc.currentMap[gPos];
                if (data.size <= tile.data.maxSize)
                {
                    tile.Select();
                    tilesInRange.Add(tile);
                }
            }
        }
    }
}
