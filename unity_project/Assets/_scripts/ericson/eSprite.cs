﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ericson
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class eSprite : MonoBehaviour
    {
        public virtual void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}

