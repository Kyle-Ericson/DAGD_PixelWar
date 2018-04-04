using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ericson;

public static class Database
{
    public static List<MapData> mapData;
    public static List<TileData> tileData;
    public static List<UnitData> unitData;
    public static List<string> tutorialData;

    private static string mapJsonPath = Application.streamingAssetsPath + "/mapdata.json";
    private static string unitJsonPath = Application.streamingAssetsPath + "/unitdata.json";
    private static string tileJsonPath = Application.streamingAssetsPath + "/tiledata.json";
    private static string tutorialJsonPath = Application.streamingAssetsPath + "/tutorialtext.json";


    public static void Load()
    {
        string mapJson = "";
        string unitJson = "";
        string tileJson = "";
        string tutorialJson = "";

        if(Application.platform == RuntimePlatform.Android)
        {
            // map data
            WWW reader = new WWW(mapJsonPath);
            while (!reader.isDone) { }
            mapJson = reader.text;

            // unit data
            reader = new WWW(unitJsonPath);
            while(!reader.isDone) { }
            unitJson = reader.text;

            // tile data
            reader = new WWW(tileJsonPath);
            while(!reader.isDone) { }
            tileJson = reader.text;

            // tutorial data
            reader = new WWW(tutorialJsonPath);
            while(!reader.isDone) { }
            tutorialJson = reader.text;

        }
        else 
        {
            mapJson = File.ReadAllText(mapJsonPath);
            unitJson = File.ReadAllText(unitJsonPath);
            tileJson = File.ReadAllText(tileJsonPath);
            tutorialJson = File.ReadAllText(tutorialJsonPath);
        }

        mapData = JsonUtility.FromJson<MapJson>(mapJson).data;
        unitData = JsonUtility.FromJson<UnitJson>(unitJson).data;
        tileData = JsonUtility.FromJson<TileJson>(tileJson).data;
        tutorialData = JsonUtility.FromJson<TutorialJson>(tutorialJson).data;
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
    public eSerializableVector2 start1;
    public eSerializableVector2 start2;
    public List<int> grid = new List<int>();
}



[System.Serializable]
public class TileJson { public List<TileData> data = new List<TileData>(); }

[System.Serializable]
public class TileData
{
    public string name;
    public TileType type;
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
    public UnitType id;
    public int range;
    public int attack;
    public int speed;
    public int size;
    public int maxHealth;
    public int maxFood;
    public int cost;
}

[System.Serializable]
public class TutorialJson { public List<string> data = new List<string>(); }



