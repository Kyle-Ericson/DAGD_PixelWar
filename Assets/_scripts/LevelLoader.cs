using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public GameObject tile;
    public GameObject tile2;

    private int[,] level;
    

    /// <summary>
	/// 
	/// </summary>
    void Start()
    {
        level = LevelData.Load(2);
        if(level != null) SpawnLevel();
    }
    /// <summary>
	/// 
	/// </summary>
    public void SpawnLevel()
    {
        float tileWidth = tile.transform.localScale.x; // tile's width
        float tileHeight = tile.transform.localScale.y; // tile's height
        float cols = level.GetLength(1); // the number of columns
        float rows = level.GetLength(0); // the number of rows
        Vector2 offset = new Vector2((cols * tileWidth * -0.5f) + (tileWidth / 2 ), (rows * tileHeight * -0.5f) + (tileHeight / 2)); // this centers the map at 0,0


        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                GameObject newTile;
                switch(level[y,x])
                {
                    case 1:
                        newTile = Instantiate(tile);
                        newTile.transform.position = new Vector2(x * tileWidth + offset.x, -y * tileHeight + -offset.y);
                        break;
                    case 2:
                        newTile = Instantiate(tile2);
                        newTile.transform.position = new Vector2(x * tileWidth + offset.x, -y * tileHeight + -offset.y);
                        break;
                }
            }
        }
    }
}
