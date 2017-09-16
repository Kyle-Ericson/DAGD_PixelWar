using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable] 
public class SerialVector2
{
    public int x;
    public int y;

    public Vector2 ToVector2() {
        return new Vector2 (x, y);
    }
}
