using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ericson;

    public class AStar : e_SingletonMono<AStar>
    {
        public AStar() { }

        public List<Tile> FindPath(Vector2 start, Vector2 target)
        {
            
            Tile startTile = MapManager.ins.currentMap[start];
            Tile targetTile = MapManager.ins.currentMap[target];

            List<Tile> openSet = new List<Tile>();
            List<Tile> closedSet = new List<Tile>();

            openSet.Add(startTile);

            while (openSet.Count > 0)
            {

                Tile currentTile = openSet[0];

                for (int i = 0; i < openSet.Count; i++)
                {
                    if ((openSet[i].f < currentTile.f) || (openSet[i].f == currentTile.f) && (openSet[i].h < currentTile.h))
                    {
                        currentTile = openSet[i];
                    }
                    openSet.Remove(currentTile);
                    closedSet.Add(currentTile);
                    
                    if (currentTile == targetTile)
                    {
                        return RetracePath(startTile, targetTile);
                    }
                }
                foreach (Tile t in MapManager.ins.GetNeighbors(currentTile))
                {
                    if (!MapManager.ins.unitGrid[start].inMoveRange.Contains(MapManager.ins.WorldToGrid(t.transform.position)) 
                    || MapManager.ins.unitGrid[start].data.size > t.data.maxSize 
                    || closedSet.Contains(t) || !MapManager.ins.currentMap.ContainsKey(t.gridpos)) continue;
                    

                    float newCost = currentTile.g + 1;
                    if ((newCost < t.g) || !openSet.Contains(t))
                    {
                        t.g = newCost;
                        t.h = Vector2.Distance(t.transform.position, targetTile.transform.position);
                        t.f = t.h + t.g;
                        t.parent = currentTile;

                        if (!openSet.Contains(t))
                        {
                            openSet.Add(t);
                        }
                    }
                }
            }
            return null;
        }
        private List<Tile> RetracePath(Tile startTile, Tile targetTile)
        {
            List <Tile> path = new List<Tile>();
            Tile currentTile = targetTile;
            while(currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = currentTile.parent;
            }
            path.Reverse();
            return path;
        }

        
        int GetDistance(Tile tile1, Tile tile2)
        {
            int dX = (int)Mathf.Abs(MapManager.ins.WorldToGrid(tile1.transform.position).x - MapManager.ins.WorldToGrid(tile2.transform.position).x);
            int dY = (int)Mathf.Abs(MapManager.ins.WorldToGrid(tile1.transform.position).y - MapManager.ins.WorldToGrid(tile2.transform.position).y);
            return dX + dY;
        }
    }
