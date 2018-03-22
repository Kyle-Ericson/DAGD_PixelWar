using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;


public class Tile : eSprite 
{
    private TileData _data = null;
    public TileData data { get { return _data; }}

    public Vector2 gridpos;
    private GameObject fog = null;
    private SpriteRenderer gridLine = null;
    private SpriteRenderer icon = null;
    public SpriteRenderer typeIcon = null;

    // AStar stuff
    public float g;
    public float h;
    public float f;
    public Tile parent;


    private void Awake()
    {
        gridLine = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        icon = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        fog = transform.GetChild(2).gameObject;
    }
    private void Start()
    {
        icon.gameObject.SetActive(false);
       
    }
    public void SetType(TileType type)
    {
        GetComponent<SpriteRenderer>().sprite = Sprites.ins.tileSprites[type];
        if (type == TileType.food) GetComponent<SpriteRenderer>().color = Color.white;
        _data = Database.tileData[(int)type];
        gridpos = MapManager.ins.WorldToGrid(transform.position);
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
    public void ShowFog()
    {
        if(fog) fog.SetActive(true);
    }
    public void HideFog()
    {
       if(fog) fog.SetActive(false);
    }
}
