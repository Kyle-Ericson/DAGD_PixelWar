using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collections
{
    // list of all map data
    private static List<MapItem> _maps;
    public static List<MapItem> maps {
        get { return _maps; }
    }
    // list of all tile data
    private static List<TileItem> _tiles;
    public static List<TileItem> tiles {
        get { return _tiles; }
    }
    // list of all unit data
    private static List<UnitItem> _units;
    public static List<UnitItem> units {
        get { return _units; }
    }

    // load all collections
    public static void Load()
    {
        _maps = JsonReader.GetMaps();
        _units = JsonReader.GetUnits();
        _tiles = JsonReader.GetTiles();
    }
}
