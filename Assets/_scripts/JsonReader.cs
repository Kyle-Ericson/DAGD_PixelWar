using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public static class JsonReader 
{
	private static string jsonMaps = File.ReadAllText(Application.dataPath + "/StreamingAssets/maps.json");
	private static string jsonUnits = File.ReadAllText(Application.dataPath + "/StreamingAssets/units.json");
	

	public static List<Unit> GetUnits() { return null; }
	public static List<Map> GetMaps() 
	{
		MapLibrary maplib =JsonUtility.FromJson<MapLibrary>(jsonMaps);
		return maplib.maps; 
	}
}
