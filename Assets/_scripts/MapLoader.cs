using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// this class get the desired level data, loads it and spawns the level into the scene
public class MapLoader : MonoBehaviour
{
    private GameObject objTile; // tile game obj
    public GameObject objGridLine; // the grid outlines
    public static MapLoader instance = null; // singleton level loader instance

    private float gridLineOffsetZ = -0.01f; // offset of the grid square outline

    void Awake()
    {
        instance = this; // variable for referencing this
        objTile = Resources.Load("prefabs/Tile", typeof(GameObject)) as GameObject; // load tile prefab
        objGridLine = Resources.Load("prefabs/GridLine", typeof(GameObject)) as GameObject; // load grid line prefab
    }
    public Dictionary<Vector2, Tile> Load (int mapID)
    {
         Map map = Libraries.maps[mapID];
         return SpawnMap(map);
    }
    private Dictionary<Vector2, Tile> SpawnMap(Map map)
    {
        if(map.grid.Count == 0) return null;
        Dictionary <Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();

        float tileWidth = objTile.transform.localScale.x; // tile's width
        float tileHeight = objTile.transform.localScale.y; // tile's height

        int cols = map.height; // the number of columns
        int rows = map.width; // the number of rows

        Vector2 offset = new Vector2( // this centers the map at 0,0 
            ((cols * tileWidth) * -0.5f) + (tileWidth * 0.5f), 
            ((rows * tileHeight) * -0.5f) + (tileHeight * 0.5f)); 

        for (var y = 0; y < rows; y++) // for each row in the grid...
        {
            for (var x = 0; x < cols; x++) // for each column in the grid...
            {
                Vector2 worldPos = new Vector2(x * tileWidth + offset.x, -y * tileHeight + -offset.y); // world space postition based on grid position
                Tile tile = Instantiate(objTile).GetComponent<Tile>(); // get tile script from tile obj
                tile.SetType(map.grid[(y * map.width) + x]); // set the type of tile based on the level data retrieved
                tile.transform.position = worldPos; // position tile in world space based on grid position
                tiles.Add(new Vector2(x,y), tile); // add the new tile to the tiles dictionary
                Instantiate(objGridLine, new Vector3(worldPos.x, worldPos.y, gridLineOffsetZ), Quaternion.identity); // spawn the grid lines                
            }
        }

        return tiles;
    }
}
