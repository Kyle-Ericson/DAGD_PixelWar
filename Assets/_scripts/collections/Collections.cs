using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collections
{
    // list of all map data
    private static List<MapItem> _mapData; 
    public static List<MapItem> mapData { get { return _mapData; } }
    // list of all tile data
    private static List<TileItem> _tileData; 
    public static List<TileItem> tileData { get { return _tileData; } }
    // list of all tile data
    private static List<UnitItem> _unitData; 
    public static List<UnitItem> unitData { get { return _unitData; } }




    // load all collections
    public static void Load()
    {
        _mapData = JsonReader.GetMaps();
        _unitData = JsonReader.GetUnits();
        _tileData = JsonReader.GetTiles();
    }
}
