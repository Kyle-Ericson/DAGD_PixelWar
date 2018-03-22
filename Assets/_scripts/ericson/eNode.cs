using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
    public class eNode
    {
        public bool isWalkable = true;
        public Vector2 pos2D;
        public Vector3 pos3D;
        public float gcost { get; private set; }
        public float hcost { get; private set; }
        public float fcost { get; private set; }
        public eNode parent { get; private set; }
        

        public void SetParent(eNode newParent)
        {
            parent = newParent;
        }
        public void SetG(float newg)
        {
            gcost = newg;
            SetF();
        }
        public void SetH(float newh)
        {
            hcost = newh;
            SetF();
        }
        private void SetF()
        {
            fcost = hcost + gcost;
        }
        public List<eNode> GetNeighbors()
        {
            return null;
        }
    }
}
