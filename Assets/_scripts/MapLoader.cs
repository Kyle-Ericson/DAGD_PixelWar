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




    void Awake()
    {
        instance = this;
        // load tile prefab
        tileObj = Resources.Load<GameObject>("prefabs/Tile");
        // load grid line prefab
        gridLineObj = Resources.Load<GameObject>("prefabs/GridLine");

        unitObj = Resources.Load<GameObject>("prefabs/Unit");
    }
    // Load the map data from the collections
    public static Dictionary<Vector2, Tile> Load(int mapID)
    {
        return MapLoader.instance.SpawnMap(Collections.maps[mapID]);
    }
    // spawn the map
    private Dictionary<Vector2, Tile> SpawnMap(MapItem map)
    {
        // if there is no grid, return
        if (map.grid.Count == 0) return null;

        // instantiate tiles dictionary
        Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();

        // get tile width and height
        float tileWidth = tileObj.transform.localScale.x;
        float tileHeight = tileObj.transform.localScale.y;

        // get number of columns and rows
        int cols = map.width;
        int rows = map.height;

        // this centers the map at 0,0
        Vector2 offset = new Vector2
        (
            ((cols * tileWidth) * -0.5f) + (tileWidth * 0.5f),
            ((rows * tileHeight) * -0.5f) + (tileHeight * 0.5f)
        );

        // for each row in the grid...
        for (var y = 0; y < rows; y++)
        {
            // for each column in the grid...
            for (var x = 0; x < cols; x++)
            {
                // skip this interation if the grid number is 0
                if (map.grid[(y * map.width) + x] == 0) continue;
                // world space postition based on grid position
                Vector2 worldPos = new Vector2(x * tileWidth + offset.x, -y * tileHeight + -offset.y);
                // spawn the grid lines
                GameObject gridLine = Instantiate(gridLineObj, new Vector3(worldPos.x, worldPos.y, gridLineOffsetZ), Quaternion.identity);
                // set the rendering layer to y
                gridLine.GetComponent<SpriteRenderer>().sortingOrder = y;
                // get tile script from tile obj
                Tile tileScript = Instantiate(tileObj).GetComponent<Tile>();
                // position tile in world space based on grid position
                tileObj.transform.position = worldPos;
                // set the type of tile based on the level data retrieved
                tileScript.SetType(map.grid[(y * map.width) + x]);
                //tileScript.GetComponent<SpriteRenderer>().sortingOrder = y;
                // add the new tile to the tiles dictionary
                tiles.Add(new Vector2(x, y), tileScript);
            }
        }
        // convert start1 into vector2
        Vector2 start1 = map.start1.ToVector2();

        // spawn start unit here
        if(tiles[start1]) {
            Instantiate(unitObj, tiles[start1].transform.position, Quaternion.identity);
        }

        return tiles;
    }
}
