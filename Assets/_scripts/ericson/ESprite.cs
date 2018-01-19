using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Ericson
{
    public class ESprite : MonoBehaviour
    {
        public virtual void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}

