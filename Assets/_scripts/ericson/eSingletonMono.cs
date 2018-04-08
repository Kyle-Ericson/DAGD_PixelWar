using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
    public class eSingletonMono<T> : MonoBehaviour where T : Component
    {

        // singleton
        private static T _ins = null;
        public static T ins
        {

            get
            {
                //GameObject check = GameObject.Find((typeof(T).ToString()));

                if (_ins == null)
                {
                    _ins = FindObjectOfType<T>();

                    if (_ins == null)
                    {
                        _ins = (new GameObject((typeof(T)).ToString())).AddComponent<T>();
                    }
                }
                return _ins;
            }
        }
        public virtual void Init()
        {

        }
        
        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void CleanUp()
        {
            
        }
    }
}