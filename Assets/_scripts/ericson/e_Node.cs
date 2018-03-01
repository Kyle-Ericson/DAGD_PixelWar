using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
    public class e_Node
    {
        public bool is_walkable = true;
        public Vector2 position_2D;
        public Vector3 position_3D;
        public float g_cost { get; private set; }
        public float h_cost { get; private set; }
        public float f_cost { get; private set; }
        public e_Node parent { get; private set; }
        

        public void Set_Parent(e_Node new_parent)
        {
            parent = new_parent;
        }
        public void Set_G(float new_g)
        {
            g_cost = new_g;
            Set_F();
        }
        public void Set_H(float new_h)
        {
            h_cost = new_h;
            Set_F();
        }
        private void Set_F()
        {
            f_cost = h_cost + g_cost;
        }
        public List<e_Node> Get_Neighbors()
        {
            return null;
        }
    }
}
