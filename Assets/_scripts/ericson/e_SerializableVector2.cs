using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ericson
{

    [System.Serializable] 
    public class e_SerializableVector2
    {
        public int x;
        public int y;

        public Vector2 ToVector2() {
            return new Vector2 (x, y);
        }
    }
}
