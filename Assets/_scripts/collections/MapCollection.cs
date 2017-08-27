using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// library for loading map data from json
[System.Serializable] public class MapCollection
{
    // list of maps from maps.json
    public List<MapItem> mapItems = new List<MapItem>(); 
}
