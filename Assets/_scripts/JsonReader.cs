using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class JsonReader // reads the json files
{

    private static string jMaps = File.ReadAllText(Application.dataPath + "/StreamingAssets/mapItems.json");
    private static string jUnits = File.ReadAllText(Application.dataPath + "/StreamingAssets/unitItems.json");
    private static string jTiles = File.ReadAllText(Application.dataPath + "/StreamingAssets/tileItems.json");


    public static List<UnitItem> GetUnits()
    {
        UnitCollection uc = JsonUtility.FromJson<UnitCollection>(jUnits); // get the collection of unit data from json
        return uc.unitItems;
    }
    public static List<MapItem> GetMaps()
    {
        MapCollection mc = JsonUtility.FromJson<MapCollection>(jMaps); // get the colleciton of map data from json
        return mc.mapItems;
    }
    public static List<TileItem> GetTiles()
    {
        TileCollection tc = JsonUtility.FromJson<TileCollection>(jTiles); // get the collection of tile data from json
        return tc.tileItems;
    }
}
