using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable] 
public class MapCollection // library for loading map data from json
{
    public List<MapItem> mapItems = new List<MapItem>(); // list of maps from maps.json
}
