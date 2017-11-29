using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collections
{
    // list of all map data
    private static MapCollection _mapCollection;
    public static MapCollection mapCollection {
        get { return _mapCollection; }
    }
    // list of all tile data
    private static TileCollection _tileCollection;
    public static TileCollection tileCollection
    {
        get { return _tileCollection; }
    }
    // list of all unit data
    private static UnitCollection _unitCollection;
    public static UnitCollection unitCollection
    {
        get { return _unitCollection; }
    }

    // load all collections
    public static void Load()
    {
        _mapCollection = JsonReader.GetMaps();
        _unitCollection = JsonReader.GetUnits();
        _tileCollection = JsonReader.GetTiles();
    }
}


[System.Serializable]
public class MapCollection { public List<MapData> mapData = new List<MapData>(); }

[System.Serializable]
public class TileCollection { public List<TileData> tileData = new List<TileData>(); }

[System.Serializable]
public class UnitCollection { public List<UnitData> unitData = new List<UnitData>(); }
