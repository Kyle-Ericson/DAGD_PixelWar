using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// reads the json files
public static class JsonReader
{
    private static string jMaps = File.ReadAllText(Application.dataPath + "/StreamingAssets/mapdata.json");
    private static string jUnits = File.ReadAllText(Application.dataPath + "/StreamingAssets/unitdata.json");
    private static string jTiles = File.ReadAllText(Application.dataPath + "/StreamingAssets/tiledata.json");

    // get the collection of unit data from json
    public static UnitCollection GetUnits()
    {
        return JsonUtility.FromJson<UnitCollection>(jUnits);
    }

    // get the colleciton of map data from json
    public static MapCollection GetMaps()
    {
        return JsonUtility.FromJson<MapCollection>(jMaps);
    }

    // get the collection of tile data from json
    public static TileCollection GetTiles()
    {
        return JsonUtility.FromJson<TileCollection>(jTiles);
    }
}
