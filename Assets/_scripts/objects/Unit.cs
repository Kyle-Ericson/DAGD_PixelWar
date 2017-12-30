using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece 
{
    
    private UnitData _data = null;
    public UnitData data { get { return _data; }}
    public int actionPoints = 3;
    public List<Tile> tilesInRange = new List<Tile>();
    

    protected override void LoadSprites() {
        //sprites.Add(0, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    public override void SetType(int type) {
        //GetComponent<SpriteRenderer>().sprite = sprites[type];
	    _data = Database.unitData[type];	
    }
    public override void Select() {
        base.Select();
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
        for (int i = -data.speed; i <= data.speed; i++)
        {
            for(int j = - data.speed; j <= data.speed; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > data.speed || i == 0 && j == 0) continue;
                var gPos = MapManager.ins.WorldToGrid(transform.position);
                gPos.x += i;
                gPos.y += j;
                if (!MapManager.ins.currentMap.ContainsKey(gPos)) continue;
                var tile = MapManager.ins.currentMap[gPos];
                if (data.size <= tile.data.maxSize)
                {
                    Debug.Log("Selecting");
                    tile.Select();
                    tilesInRange.Add(tile);
                }
            }
        }
    }
}
