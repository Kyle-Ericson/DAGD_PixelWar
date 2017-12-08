using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable] 
public class TileData : Data
{
    public TileType id;
    public string name;
    public int defense;
    public int moveCost;
    public int maxSize;
}


public enum TileType
{
    plain = 1,
    forest = 2,
    mountain = 3
}
