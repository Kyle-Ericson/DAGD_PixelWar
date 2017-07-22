using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Libraries {

	private static List<Map> _maps; // list of all the maps
	public static List<Map> maps { get {return _maps; } } // maps getter
	
	
	public static void Load() // load all libraries
	{ 
		LoadMaps(); // load maps
		LoadUnits(); // load units
	} 
		
	private static void LoadMaps() { _maps = JsonReader.GetMaps(); } // load all maps from json file
	private static void LoadUnits() { } // load all unit data from json
}
