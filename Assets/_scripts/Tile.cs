using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;


public class Tile : ESprite 
{
    private TileData _data = null;
    public TileData data { get { return _data; }}
    private SpriteRenderer gridLine = null;
    private SpriteRenderer icon = null;


    // AStar stuff
    public float g;
    public float h;
    public float f;
    public Tile parent;



    private void Start()
    {
        icon = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        gridLine = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        icon.gameObject.SetActive(false);
    }
    public void SetType(TileType type)
    {
        GetComponent<SpriteRenderer>().sprite = Sprites.ins.tileSprites[type];
        _data = Database.tileData[(int)type];
    }
    public void Highlight()
    {
        icon.gameObject.SetActive(true);
    }
    public void UnHighlight()
    {
        icon.gameObject.SetActive(false);
    }
    public void SetIconColor(Color color)
    {
        icon.color = color;
    }
    public void ShowGrid()
    {
        gridLine.gameObject.SetActive(true);
    }
    public void HideGrid()
    {
        gridLine.gameObject.SetActive(false);
    }
}
