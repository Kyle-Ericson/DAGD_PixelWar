using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ericson;

// this class get the desired level data, loads it and spawns the level into the scene
public class MapManager : eSingletonMono<MapManager>
{
    // current map data
    public MapData currentMapData;
    // current map's tiles
    public Dictionary<Vector2, Tile> currentMap = new Dictionary<Vector2, Tile>();
    public Dictionary<Vector2, Unit> unitGrid = new Dictionary<Vector2, Unit>();
    // tile scale
    public float _tileScale = 1;
    // tile game obj
    private GameObject tilePrefab;
    private GameObject water;
    private GameObject waterHolder;





    void Awake()
    {
        tilePrefab = Resources.Load<GameObject>("prefabs/Tile");
        water = Resources.Load<GameObject>("prefabs/water");
        waterHolder = new GameObject("Water");
    }
    public void SpawnMap(int mapID)
    {
        currentMapData = Database.mapData[mapID];
        currentMap.Clear();
        // if there is no grid, return
        if (currentMapData.grid.Count == 0) { return; }

        // for each row in the grid...
        for (var y = 0; y < currentMapData.rows; y++)
        {
            // for each column in the grid...
            for (var x = 0; x < currentMapData.cols; x++)
            {
                int tileType = currentMapData.grid[OneDGridPos(x,y)];

                // skip this interation if the grid number is 0
                if (tileType == 0) 
                {
                    Vector2 wp = GridToWorld(new Vector2(x, y));
                    GameObject t = Instantiate(water, new Vector3(wp.x, wp.y, 0), Quaternion.identity);
                    t.transform.position = new Vector3(wp.x, wp.y, 0);
                    t.transform.SetParent(waterHolder.transform);
                    continue;
                }

                // world space postition based on grid position
                Vector2 worldPos = GridToWorld(new Vector2(x, y));
                
                // get tile script from tile obj
                Tile tile = Instantiate(tilePrefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity).GetComponent<Tile>();
                tile.gameObject.transform.SetParent(gameObject.transform);
                tile.name = "(" + x + ", " + y + ")";

                // set the type of tile based on the level data retrieved
                
                switch(tileType)
                {
                    case 0:
                        break;
                    case 1:
                        tile.SetType(TileType.grassMid);
                        break;
                    case 2:
                        tile.SetType(TileType.wall);
                        break;
                    case 3:
                        tile.SetType(TileType.food);
                        break;
                }
                if (!PersistentSettings.useGrid) tile.HideGrid();
                //tile.GetComponent<SpriteRenderer>().sortingOrder = y;

                // add the new tile to the tiles dictionary
                currentMap.Add(new Vector2(x, y), tile);
            }
        }
        SetCamera();
    }
    // TODO: Move this somewhere that makes more sense.
    // Move the camera to the center of the map.
    private void SetCamera()
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


    public List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if((x == 0 && y == 0) || Mathf.Abs(x) + Mathf.Abs(y) > 1) continue;

                Vector2 check = WorldToGrid(tile.transform.position);
                check.x += x;
                check.y += y;

                if (MapManager.ins.currentMap.ContainsKey(check))
                {
                    neighbors.Add(MapManager.ins.currentMap[check]);
                }
            }
        }
        return neighbors;
    }
    public void RemoveUnit(Vector2 pos)
    {
        Destroy(unitGrid[pos].gameObject);
        unitGrid.Remove(pos);
    }
    public void RemoveTile(Vector2 pos)
    {
        Destroy(currentMap[pos].gameObject);
        currentMap.Remove(pos);
    }
    //
    public void RemoveAllUnits()
    {
        List<Vector2> temp_list = new List<Vector2>();
        foreach (KeyValuePair<Vector2, Unit> item in unitGrid) { temp_list.Add(item.Key); }
        foreach (Vector2 v in temp_list) RemoveUnit(v);
        unitGrid.Clear();
    }
    // remove all tiles from the scene
    private void RemoveAllTiles()
    {
        List<Vector2> temp_list = new List<Vector2>();
        foreach (KeyValuePair<Vector2, Tile> item in currentMap) { temp_list.Add(item.Key); }
        foreach (Vector2 v in temp_list) RemoveTile(v);
        for(int i = waterHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(waterHolder.transform.GetChild(i).gameObject);
        }
        currentMap.Clear();
    }
    public override void CleanUp()
    {
        base.CleanUp();
        RemoveAllTiles();
        RemoveAllUnits();
    }
}
