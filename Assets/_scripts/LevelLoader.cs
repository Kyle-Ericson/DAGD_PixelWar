using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// this class get the desired level data, loads it and spawns the level into the scene
public class LevelLoader : MonoBehaviour
{
    public GameObject tile; // the tile object
    public GameObject gridSquare; // the grid outlines
    public static LevelLoader instance = null; // singleton level loader instance

    private float gridSquareZ = -0.01f; // offset of the grid square outline
    private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();// dictionary for holding a reference to all the tiles in the grid
    private int[,] level; // this holds the level data retrieved from LevelData


    void Awake()
    {
        instance = this; // variable for referencing this
    }
    public Dictionary<Vector2, Tile> Load (int levelNum) 
    {
        level = LevelData.GetLevel(levelNum); // get the level data
        SpawnLevel(); // spawn the level into the scene
        return tiles; // return a reference to all of the tiles
    }
    private void SpawnLevel()
    {
        if(tiles.Count == 0) RemoveLevel();
        float tileWidth = tile.transform.localScale.x; // tile's width
        float tileHeight = tile.transform.localScale.y; // tile's height
        float cols = level.GetLength(1); // the number of columns
        float rows = level.GetLength(0); // the number of rows
        Vector2 offset = new Vector2( // this centers the map at 0,0 
            (cols * tileWidth * -0.5f) + (tileWidth / 2 ), 
            (rows * tileHeight * -0.5f) + (tileHeight / 2)); 

        for (var y = 0; y < rows; y++) // for each row in the grid...
        {
            for (var x = 0; x < cols; x++) // for each column in the grid...
            {
                Vector2 worldPos = new Vector2(x * tileWidth + offset.x, -y * tileHeight + -offset.y); // world space postition based on grid position
                Tile tileScript = Instantiate(tile).GetComponent<Tile>(); // get tile script from tile obj
                tileScript.SetType(level[y,x]); // set the type of tile based on the level data retrieved
                tileScript.gridPosition = new Vector2(x,y);
                tileScript.transform.position = worldPos; // position tile in world space based on grid position
                tiles.Add(new Vector2(x,y), tileScript); // add the new tile to the tiles dictionary
                Instantiate(gridSquare, new Vector3(worldPos.x, worldPos.y, gridSquareZ), Quaternion.identity); // spawn the grid lines                
            }
        }
    }
    private void RemoveLevel()
    {
        foreach(KeyValuePair<Vector2, Tile> item in tiles)
        {
            Destroy(item.Value.gameObject);
        }
        tiles.Clear(); // clear tiles dictionary
    }
}
