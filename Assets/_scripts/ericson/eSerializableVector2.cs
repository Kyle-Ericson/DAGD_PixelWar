using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ericson
{

    [System.Serializable] 
    public class eSerializableVector2
    {
        public float x;
        public float y;

        public eSerializableVector2() { }

        public eSerializableVector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public Vector2 ToVector2() 
        {
            return new Vector2 (x, y);
        }
        public static eSerializableVector2 FromVector2(Vector2 v)
        {
            return new eSerializableVector2(v.x, v.y);
        }
    }
}
