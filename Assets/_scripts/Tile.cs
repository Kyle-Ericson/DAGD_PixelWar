using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class Tile : MonoBehaviour { // the tiles of the map

	public bool isSelected = false; // if this tile is currently selected

	void OnMouseOver() { isSelected = true; } // if mouse is over this tile, it is selected
	void OnMouseExit() { isSelected = false; } // if mouse moves off this tile, it is no longer selected

	public void SetType(int typeNum) // set the type of tile that should be used
	{
		
	}
}
