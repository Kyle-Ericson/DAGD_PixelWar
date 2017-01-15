using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GridGenerator
{

    /// <summary>
    /// This creates the grid and returns a vector2 array of the locations.
    /// </summary>
    public static Vector2[,] GenerateGrid(int cols, int rows, Vector2 position)
    {
        Vector2[,] grid = new Vector2[cols,rows];

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {

                Vector2 cell = new Vector2(x + position.x, y + position.y);
                grid[x,y] = cell;

            }
        }
        return grid;
    }
}
