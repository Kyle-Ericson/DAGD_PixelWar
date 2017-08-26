using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable] // library for loading map data from json
public class MapCollection
{
    public List<MapItem> mapItems = new List<MapItem>(); // list of maps from maps.json
}
