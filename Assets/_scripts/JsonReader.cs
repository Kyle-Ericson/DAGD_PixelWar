using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class JsonReader { // reads the json files

    private static string jMaps = File.ReadAllText(Application.dataPath + "/StreamingAssets/maps.json");
    private static string jUnits = File.ReadAllText(Application.dataPath + "/StreamingAssets/units.json");
    private static string jTiles = File.ReadAllText(Application.dataPath + "/StreamingAssets/tiles.json");


    public static List<UnitData> GetUnits() {
        UnitCollection uc = JsonUtility.FromJson<UnitCollection>(jUnits); // get the collection of unit data from json
        return uc.units;
    }
    public static List<MapData> GetMaps() {
        MapCollection mc = JsonUtility.FromJson<MapCollection>(jMaps); // get the colleciton of map data from json
        return mc.maps;
    }
    public static List<TileData> GetTiles() {
        TileCollection tc = JsonUtility.FromJson<TileCollection>(jTiles); // get the collection of tile data from json
        return tc.tiles;
    }
}
