using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable] public class MapCollection // library for loading map data from json
{
    public List<MapData> maps = new List<MapData>();     // list of maps from maps.json
}
