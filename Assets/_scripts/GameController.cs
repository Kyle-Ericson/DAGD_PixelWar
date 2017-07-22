using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	
	[HideInInspector] public static GameController instance = null; // singleton of game controller
	private Selector selector; // reference to the selector
	public Dictionary<Vector2, Tile> tiles; // list of tiles of the current map
	



	void Awake ()
	{
		GameController.instance = this; // singleton gamecontroller instance
		selector = GameObject.Find("Selector").GetComponent<Selector>(); // get reference to the selector
	}

	void Start () 
	{
		Libraries.Load(); // load all the libraries
		tiles = MapLoader.instance.Load(0); // spawn the map and load the tiles
	}
	
	void Update () 
	{
		MoveSelector();
	}

	private void RemoveMap() // remove the map from the game
    {
        foreach(KeyValuePair<Vector2, Tile> item in tiles) // for each tile...
        {
            Destroy(item.Value.gameObject); // destroy the game object
        }
        tiles.Clear(); // clear tiles dictionary
    }
	public void MoveSelector () // move the selector based on which tile is selected
	{
		foreach(KeyValuePair<Vector2, Tile> k in tiles) // for each tile...
		{
			if(k.Value.isSelected)
			{
				Debug.Log(k.Value);
				selector.Move(k.Value.transform.position); // check if it is selected, then move the selector
			}
		}
	}
}
