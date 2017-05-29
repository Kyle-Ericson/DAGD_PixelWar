using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelData {

	// level 1
	public static int[,] level_1 =
	{
		{1,1,1,1,1},
		{1,1,1,1,1},
		{1,1,2,1,1},
		{1,1,1,1,1},
		{1,1,1,1,1}
	};
	// level 2
	public static int[,] level_2 =
	{
		{2,2,1,1,1,1,1,1},
		{1,2,2,1,1,1,1,1},
		{1,1,2,1,1,1,1,1},
		{1,1,2,1,1,1,1,1},
		{1,1,2,2,2,2,2,1},
		{1,1,1,1,1,1,2,2}
	};



	/// <summary>
	/// Returns the level data.
	/// </summary>
	public static int[,] Load(int level) {
		switch(level) {
			case 1: return level_1;
			case 2: return level_2;
		}
		return null;
	}
}
