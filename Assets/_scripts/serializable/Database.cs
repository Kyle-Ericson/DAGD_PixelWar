using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Database
{
    public static List<MapData> mapData;
    public static List<TileData> tileData;
    public static List<UnitData> unitData;

    private static string mapJsonPath = Application.dataPath + "/StreamingAssets/mapdata.json";
    private static string unitJsonPath = Application.dataPath + "/StreamingAssets/unitdata.json";
    private static string tileJsonPath = Application.dataPath + "/StreamingAssets/tiledata.json";
    

    public static void Load()
    {
        string mapJson = File.ReadAllText(mapJsonPath);
        string unitJson = File.ReadAllText(unitJsonPath);
        string tileJson = File.ReadAllText(tileJsonPath);

        mapData = JsonUtility.FromJson<MapJson>(mapJson).data;
        unitData = JsonUtility.FromJson<UnitJson>(unitJson).data;
        tileData = JsonUtility.FromJson<TileJson>(tileJson).data;

    }
}


[System.Serializable]
public class MapJson { public List<MapData> data = new List<MapData>(); }

[System.Serializable]
public class MapData
{
    public int id;
    public string name;
    public int cols;
    public int rows;
    public SerialVector2 start1;
    public SerialVector2 start2;
    public List<int> grid = new List<int>();
}



[System.Serializable]
public class TileJson { public List<TileData> data = new List<TileData>(); }

[System.Serializable]
public class TileData
{
    public string name;
    public int defense;
    public int moveCost;
    public int maxSize;
}



[System.Serializable]
public class UnitJson { public List<UnitData> data = new List<UnitData>(); }

[System.Serializable]
public class UnitData
{
    public string name;
    public int tier;
    public int range;
    public int attack;
    public int speed;
    public int size;
}



