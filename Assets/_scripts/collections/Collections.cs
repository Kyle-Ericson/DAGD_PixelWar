using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collections
{

    private static List<MapData> _maps; // list of all map data
    public static List<MapData> maps { get { return _maps; } }
    private static List<TileData> _tiles; // list of all tile data
    public static List<TileData> tiles { get { return _tiles; } }
    private static List<UnitData> _units; // list of all tile data
    public static List<UnitData> units { get { return _units; } }


    public static void Load() { // load all collections
        LoadMaps(); // load maps
        LoadUnits(); // load units
        LoadTiles();
    }
    private static void LoadMaps() {
        _maps = JsonReader.GetMaps(); // load all maps from JsonReader 
    }
    private static void LoadUnits() {
         _units = JsonReader.GetUnits();// load all unit data from JsonReader
    }
    private static void LoadTiles() {
        _tiles = JsonReader.GetTiles(); // load all tile data from JsonReader
        
    }
}
