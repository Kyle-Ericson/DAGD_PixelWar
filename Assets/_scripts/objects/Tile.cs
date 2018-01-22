using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;


public class Tile : ESprite 
{
    private TileData _data = null;
    public TileData data { get { return _data; }}
    private Color startColor;
    public float g;
    public float h;
    public float f;

    public Tile parent;



    private void Start()
    {
        startColor = GetComponent<SpriteRenderer>().color;
    }
    public void SetType(TileType type)
    {
        GetComponent<SpriteRenderer>().sprite = Sprites.ins.tileSprites[type];
        _data = Database.tileData[(int)type];
    }
    public void Highlight()
    {
        SetColor(Color.green);
    }
    public void UnHighlight()
    {
        SetColor(startColor);
    }

}
