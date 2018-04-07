using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ericson;


public class Tile : eSprite 
{
    private TileData _data = null;
    public TileData data { get { return _data; }}

    public Vector2 gridpos;
    private GameObject fog = null;
    private SpriteRenderer gridLine = null;
    private SpriteRenderer icon = null;
    public TextMeshPro textIcon = null;
    public SpriteRenderer typeIcon = null;
    public bool highlighted = false;
    // AStar stuff
    public float g;
    public float h;
    public float f;
    public Tile parent;
    public Sprite grassMiddle = null;
    public Sprite grassEdge = null;
    public Sprite grassCorner = null;
    public Sprite wall = null;


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
        _data = Database.tileData[(int)type];
        gridpos = MapManager.ins.WorldToGrid(transform.position);
        //if((int)type == 1) CheckNeighbors();
    }
    public void Highlight()
    {
        icon.gameObject.SetActive(true);
        textIcon.gameObject.SetActive(false);
        highlighted = true;
    }
    public void Highlight(string text)
    {
        textIcon.gameObject.SetActive(true);
        icon.gameObject.SetActive(false);
        textIcon.text = text;
        highlighted = true;
    }
    public void UnHighlight()
    {
        icon.gameObject.SetActive(false);
        textIcon.gameObject.SetActive(false);
        highlighted = false;
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
    public void CheckNeighbors()
    {
        Debug.Log("Checking");
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // left corner
        if(!CheckTop() && !CheckLeft()) 
        { 
            sr.sprite = Sprites.ins.tileSprites[TileType.grassCorner]; 
            sr.flipX = true;
        }
        // right corner
        else if(!CheckTop() && !CheckRight()) 
        { 
            sr.sprite = Sprites.ins.tileSprites[TileType.grassCorner];
        }
        // right bottom
        else if(!CheckRight() && !CheckBottom())
        { 
            sr.sprite = Sprites.ins.tileSprites[TileType.grassCorner]; 
            sr.flipY = true; 
        }
        // left bottom 
        else if(!CheckLeft() && !CheckBottom())
        { 
            sr.sprite = Sprites.ins.tileSprites[TileType.grassCorner]; 
            sr.flipY = true; 
            sr.flipX = true; 
        }
        
    }
    public bool CheckTop()
    {
        if(gridpos.y == 0) return false;
        int tileType = MapManager.ins.OneDGridPos((int)gridpos.x, (int)gridpos.y - 1);
        if(tileType == 0) return false;
        return true;
    }
    public bool CheckBottom()
    {
        if(gridpos.y == MapManager.ins.currentMapData.rows - 1) return false;
        int tileType = MapManager.ins.OneDGridPos((int)gridpos.x, (int)gridpos.y + 1);
        if(tileType == 0) return false;
        
        return true;
    }
    public bool CheckLeft()
    {
        if(gridpos.x == 0) return false;
        int tileType = MapManager.ins.OneDGridPos((int)gridpos.x - 1, (int)gridpos.y);
        if(tileType == 0) return false;
        return true;
    }
    public bool CheckRight()
    {
        if(gridpos.x == MapManager.ins.currentMapData.cols - 1) return false;
        int tileType = MapManager.ins.OneDGridPos((int)gridpos.x + 1, (int)gridpos.y);
        if(tileType == 0) return false;
        return true;
    }
}
