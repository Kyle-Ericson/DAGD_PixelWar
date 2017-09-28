using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable] 
public class MapItem : Item
{
    public int id;
    public string name;
    public int width;
    public int height;
    public SerialVector2 start1;
    public SerialVector2 start2;
    public List<int> grid = new List<int>();
}
