using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// library for tile data
[System.Serializable] public class TileCollection 
{ 
    // list of maps from maps.json
    public List<TileItem> tileItems = new List<TileItem>();
}
