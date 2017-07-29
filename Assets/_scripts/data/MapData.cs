using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class MapData
{
    public int id;
    public string name;
    public int width;
    public int height;
    public List<int> grid = new List<int>();
}
