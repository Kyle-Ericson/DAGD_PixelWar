using UnityEngine;
using System.Collections;

public class MapMaker : MonoBehaviour
{
    public GameObject tile;
    


    void Start()
    {
        SpawnMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnMap()
    {
        Vector2[,] grid = GridGenerator.GenerateGrid(20, 10, Vector2.zero);


        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {

                var newTile = Instantiate(tile);

                newTile.transform.position = grid[x,y];
            }

        }
        
    }
}
