using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	
	[HideInInspector] public GameController instance = null; // singleton of game controller
	private LevelLoader levelloader; // reference to the level loader
	private Selector selector; // reference to the selector
	private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>(); // variable for holding a reference to all of the grid tiles



	void Awake ()
	{
		instance = this;
		selector = GameObject.Find("Selector").GetComponent<Selector>(); // get reference to the selector
	}

	// Use this for initialization
	void Start () 
	{
		levelloader = LevelLoader.instance; // get instance to the level loader
		if(levelloader) // if level loader exists...
		{
			tiles = levelloader.Load(1); // load the level and get reference to the tiles
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void MoveSelector (Vector2 position)
	{
		selector.Move(position);
	}
}
