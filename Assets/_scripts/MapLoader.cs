using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// this class get the desired level data, loads it and spawns the level into the scene
public class MapLoader : MonoBehaviour
{
    private GameObject Tile; // tile game obj
    public GameObject objGridLine; // the grid outlines
    public static MapLoader instance = null; // singleton level loader instance

    private float gridLineOffsetZ = -0.01f; // offset of the grid square outline

    void Awake() {
        instance = this; // variable for referencing this
        Tile = Resources.Load<GameObject>("prefabs/Tile"); // load tile prefab
        objGridLine = Resources.Load<GameObject>("prefabs/GridLine"); // load grid line prefab
    }
    public Dictionary<Vector2, Tile> Load (int mapID) {
        MapData map = Collections.maps[mapID];  // get the map from the library
        return SpawnMap(map);  // spaw the map
    }
    private Dictionary<Vector2, Tile> SpawnMap(MapData map) {
        if(map.grid.Count == 0) return null; // if there is no grid, return
        Dictionary <Vector2, Tile> tiles = new Dictionary<Vector2, Tile>(); // instantiate tiles dictionary

        float tileWidth = Tile.transform.localScale.x; // tile's width
        float tileHeight = Tile.transform.localScale.y; // tile's height

        int cols = map.width; // the number of columns
        int rows = map.height; // the number of rows

        Vector2 offset = new Vector2( // this centers the map at 0,0
            ((cols * tileWidth) * -0.5f) + (tileWidth * 0.5f),
            ((rows * tileHeight) * -0.5f) + (tileHeight * 0.5f));

        for (var y = 0; y < rows; y++) { // for each row in the grid...
            for (var x = 0; x < cols; x++){ // for each column in the grid...
                if(map.grid[(y * map.width) + x] == 0) continue;
                Vector2 worldPos = new Vector2(x * tileWidth + offset.x, -y * tileHeight + -offset.y); // world space postition based on grid position
                Tile tile = Instantiate(Tile).GetComponent<Tile>(); // get tile script from tile obj
                tile.transform.position = worldPos; // position tile in world space based on grid position
                tile.SetType(map.grid[(y * map.width) + x]); // set the type of tile based on the level data retrieved
                tile.GetComponent<SpriteRenderer>().sortingOrder = y;
                tiles.Add(new Vector2(x,y), tile); // add the new tile to the tiles dictionary
                Instantiate(objGridLine, new Vector3(worldPos.x, worldPos.y, gridLineOffsetZ), Quaternion.identity); // spawn the grid lines
            }
        }
        return tiles;
    }
}
