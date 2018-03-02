using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ericson
{
    public class e_AStar : e_SingletonMono<e_AStar>
    {
        public e_AStar() { }

        public List<e_Node> FindPath(e_Node start_node, e_Node target_node)
        {
            
            List<e_Node> open_set = new List<e_Node>();
            List<e_Node> closed_set = new List<e_Node>();

            open_set.Add(start_node);

            while (open_set.Count > 0)
            {

                e_Node current_node = open_set[0];

                for (int i = 1; i < open_set.Count; i++)
                {
                    if ((open_set[i].f_cost < current_node.f_cost) || (open_set[i].f_cost == current_node.f_cost)
                        && (open_set[i].h_cost < current_node.h_cost))
                    {
                        current_node = open_set[i];
                    }
                    open_set.Remove(current_node);
                    closed_set.Add(current_node);

                    if (current_node == target_node)
                    {
                        return RetracePath(start_node, target_node);
                    }
                }
                foreach (e_Node node in current_node.Get_Neighbors())
                {
                    if (!node.is_walkable || closed_set.Contains(node)) continue;

                    float new_cost = current_node.g_cost + Get_Distance(current_node, node);
                    if ((new_cost < node.g_cost) || !open_set.Contains(node))
                    {
                        node.Set_G(new_cost);
                        node.Set_H(Get_Distance(node, target_node));
                        node.Set_Parent(current_node);

                        if (!open_set.Contains(node))
                        {
                            open_set.Add(node);
                        }
                    }
                }
            }
            return null;
        }
        private List<e_Node> RetracePath(e_Node start_node, e_Node target_node)
        {
            List<e_Node> path = new List<e_Node>();
            e_Node current_node = target_node;
            while (current_node != start_node)
            {
                path.Add(current_node);
                current_node = current_node.parent;
            }
            path.Reverse();
            return path;
        }


        float Get_Distance(e_Node node_a, e_Node node_b)
        {
            float dX = Mathf.Abs(node_a.position_2D.x - node_b.position_2D.x);
            float dY = Mathf.Abs(node_a.position_2D.y - node_b.position_2D.y);

            if (dX > dY) { return dY + 10 * (dX - dY); }
            return dX + 10 * (dY - dX);
        }
    }
}
