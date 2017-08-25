using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collections
{

    private static List<MapItem> _mapData; // list of all map data
    public static List<MapItem> mapData { get { return _mapData; } }
    private static List<TileItem> _tileData; // list of all tile data
    public static List<TileItem> tileData { get { return _tileData; } }
    private static List<UnitItem> _unitData; // list of all tile data
    public static List<UnitItem> unitData { get { return _unitData; } }


    public static void Load() { // load all collections
        _mapData = JsonReader.GetMaps(); // load all maps from JsonReader 
        _unitData = JsonReader.GetUnits();// load all unit data from JsonReader
        _tileData = JsonReader.GetTiles(); // load all tile data from JsonReader
    }
}
