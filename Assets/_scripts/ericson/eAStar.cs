using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ericson
{
    public class eAStar : eSingletonMono<eAStar>
    {
        public eAStar() { }

        public List<eNode> FindPath(eNode startNode, eNode targetNode)
        {
            
            List<eNode> openSet = new List<eNode>();
            List<eNode> closedSet = new List<eNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {

                eNode currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if ((openSet[i].fcost < currentNode.fcost) || (openSet[i].fcost == currentNode.fcost)
                        && (openSet[i].hcost < currentNode.hcost))
                    {
                        currentNode = openSet[i];
                    }
                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        return RetracePath(startNode, targetNode);
                    }
                }
                foreach (eNode node in currentNode.GetNeighbors())
                {
                    if (!node.isWalkable || closedSet.Contains(node)) continue;

                    float newCost = currentNode.gcost + GetDistance(currentNode, node);
                    if ((newCost < node.gcost) || !openSet.Contains(node))
                    {
                        node.SetG(newCost);
                        node.SetH(GetDistance(node, targetNode));
                        node.SetParent(currentNode);

                        if (!openSet.Contains(node))
                        {
                            openSet.Add(node);
                        }
                    }
                }
            }
            return null;
        }
        private List<eNode> RetracePath(eNode startNode, eNode targetNode)
        {
            List<eNode> path = new List<eNode>();
            eNode currentNode = targetNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }


        float GetDistance(eNode nodeA, eNode nodeB)
        {
            float dX = Mathf.Abs(nodeA.pos2D.x - nodeB.pos2D.x);
            float dY = Mathf.Abs(nodeA.pos2D.y - nodeB.pos2D.y);

            if (dX > dY) { return dY + 10 * (dX - dY); }
            return dX + 10 * (dY - dX);
        }
    }
}
