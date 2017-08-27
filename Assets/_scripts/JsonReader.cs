using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// reads the json files
public static class JsonReader 
{

    private static string jMaps = File.ReadAllText(Application.dataPath + "/StreamingAssets/mapItems.json");
    private static string jUnits = File.ReadAllText(Application.dataPath + "/StreamingAssets/unitItems.json");
    private static string jTiles = File.ReadAllText(Application.dataPath + "/StreamingAssets/tileItems.json");

    // get the collection of unit data from json
    public static List<UnitItem> GetUnits()
    {
        UnitCollection uc = JsonUtility.FromJson<UnitCollection>(jUnits); 
        return uc.unitItems;
    }
     // get the colleciton of map data from json
    public static List<MapItem> GetMaps()
    {
        MapCollection mc = JsonUtility.FromJson<MapCollection>(jMaps);
        return mc.mapItems;
    }
    // get the collection of tile data from json
    public static List<TileItem> GetTiles()
    {
        TileCollection tc = JsonUtility.FromJson<TileCollection>(jTiles); 
        return tc.tileItems;
    }
}
