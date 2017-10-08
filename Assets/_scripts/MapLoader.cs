using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// this class get the desired level data, loads it and spawns the level into the scene
public class MapLoader : MonoBehaviour
{
    // tile game obj
    private GameObject tileObj;
    // unit game obj
    private GameObject unitObj;
    // the grid outlines
    private GameObject gridLineObj;
    // maploader reference to itself
    [HideInInspector] public static MapLoader instance = null;
    // offset of the grid square outline
    private float gridLineOffsetZ = -0.01f;
    // current map data
    public static MapItem currentMap;
    // tile scale
    float tileWidth;
    float tileHeight;




    void Awake()
    {
        instance = this;
        // load tile prefab
        tileObj = Resources.Load<GameObject>("prefabs/Tile");
        // load grid line prefab
        gridLineObj = Resources.Load<GameObject>("prefabs/GridLine");
    }
    // Load the map data from the collections
    public Dictionary<Vector2, Tile> LoadMap(int mapID)
    {
        return MapLoader.instance.SpawnMap(Collections.maps[mapID]);
    }
    // spawn the map
    private Dictionary<Vector2, Tile> SpawnMap(MapItem map)
    {
        currentMap = map;
        // if there is no grid, return
        if (map.grid.Count == 0) return null;
        // instantiate tiles dictionary
        Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
        // get tile width and height
        tileWidth = tileObj.transform.localScale.x;
        tileHeight = tileObj.transform.localScale.y;
        // for each row in the grid...
        for (var y = 0; y < map.rows; y++)
        {
            // for each column in the grid...
            for (var x = 0; x < map.cols; x++)
            {
                // skip this interation if the grid number is 0
                if (map.grid[(y * map.cols) + x] != 0) {
                    // world space postition based on grid position
                    Vector2 worldPos = new Vector2(x + (tileWidth / 2), -y - (tileHeight / 2));
                    // spawn the grid lines
                    GameObject gridLine = Instantiate(gridLineObj, new Vector3(worldPos.x, worldPos.y, gridLineOffsetZ), Quaternion.identity);
                    // set the rendering layer to y
                    gridLine.GetComponent<SpriteRenderer>().sortingOrder = y;
                    // get tile script from tile obj
                    Tile tileScript = Instantiate(tileObj, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity).GetComponent<Tile>();
                    // set the type of tile based on the level data retrieved
                    tileScript.SetType(map.grid[(y * map.cols) + x]);
                    //tileScript.GetComponent<SpriteRenderer>().sortingOrder = y;
                    // add the new tile to the tiles dictionary
                    tiles.Add(new Vector2(x, y), tileScript);
                }
            }
        }
        MoveCamera();
        return tiles;
    }
    private void MoveCamera() {
        // move the camera
        float camX = (currentMap.cols * tileWidth) / 2;
        float camY = -(currentMap.rows * tileHeight) / 2;
        Camera.main.transform.position = new Vector3(camX,camY, -10);
    }
}
