using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// this class get the desired level data, loads it and spawns the level into the scene
public class MapManager : MonoBehaviour
{
    // singleton
    private static MapManager _ins = null;
    public static MapManager ins
    {
        get
        {
            if(_ins == null)
            {
                _ins = (new GameObject("MapManager")).AddComponent<MapManager>();
            }
            return _ins;
        }
            
    }

    // current map data
    public MapData currentMapData;
    // current map's tiles
    public Dictionary<Vector2, Tile> currentMap = new Dictionary<Vector2, Tile>();
    // tile scale
    public float _tileScale = 1;
    // tile game obj
    private GameObject tilePrefab;
    // the grid outlines
    private GameObject gridLineObj;    
    // offset of the grid square outline
    private float gridLineOffsetZ = -0.01f;



    void Awake()
    {
        tilePrefab = Resources.Load<GameObject>("prefabs/Tile");
        gridLineObj = Resources.Load<GameObject>("prefabs/GridLine");
    }
    public void SpawnMap(int mapID)
    {
        currentMapData = Database.mapData[mapID];
        // if there is no grid, return
        if (currentMapData.grid.Count == 0) { return; }

        // for each row in the grid...
        for (var y = 0; y < currentMapData.rows; y++)
        {
            // for each column in the grid...
            for (var x = 0; x < currentMapData.cols; x++)
            {
                int oneDGridPos = currentMapData.grid[OneDGridPos(x,y)];

                // skip this interation if the grid number is 0
                if (currentMapData.grid[oneDGridPos] == 0) continue;

                // world space postition based on grid position
                Vector2 worldPos = GridToWorld(new Vector2(x, y));

                // spawn the grid lines
                Instantiate(gridLineObj, new Vector3(worldPos.x, worldPos.y, gridLineOffsetZ), Quaternion.identity);

                // get tile script from tile obj
                Tile tile = Instantiate(tilePrefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity).GetComponent<Tile>();

                // set the type of tile based on the level data retrieved
                tile.SetType(oneDGridPos);
                //tile.GetComponent<SpriteRenderer>().sortingOrder = y;

                // add the new tile to the tiles dictionary
                currentMap.Add(new Vector2(x, y), tile);
            }
        }
        MoveCamera();
    }
    // TODO: Move this somewhere that makes more sense.
    // Move the camera to the center of the map.
    private void MoveCamera()
    {
        float camX = (currentMapData.cols * _tileScale) / 2;
        float camY = -(currentMapData.rows * _tileScale) / 2;
        Camera.main.transform.position = new Vector3(camX, camY, -10);
    }
    // The json file stores the map's grid data as a one dimensional array.
    // We can access it with (x, y) coords using this method.
    public int OneDGridPos (int x, int y)
    {
        return (y * currentMapData.cols) + x;
    }
    // Convert a world position into a grid position.
    public Vector2 WorldToGrid(Vector2 worldPos)
    {
        return new Vector2(Mathf.Floor(worldPos.x / _tileScale), -Mathf.Floor(worldPos.y / _tileScale) - 1);
    }
    // Convert a grid position into a world position.
    public Vector3 GridToWorld(Vector2 gridPos)
    {
        return new Vector3(gridPos.x * _tileScale + (_tileScale / 2), -gridPos.y * _tileScale - (_tileScale / 2), 0);
    }
}
