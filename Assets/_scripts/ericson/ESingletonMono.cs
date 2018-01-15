using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ericson
{
    public class ESingletonMono<T> : MonoBehaviour where T : Component
    {

        // singleton
        private static T _ins = null;
        public static T ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = (new GameObject((typeof(T)).ToString())).AddComponent<T>();
                }
                return _ins;
            }
        }
    }
}