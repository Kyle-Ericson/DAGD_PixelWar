using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	
	public Vector2 gridPosition;
	public bool isSelected = false;

	private GameController gc;

	
	void Awake()
	{
		gc = GameObject.Find("Script Holder").GetComponent<GameController>();
	}

	void Start() 
	{
		
	}

	void Update()
	{
		if(isSelected) gc.MoveSelector(transform.position);
	}

	void OnMouseOver() { isSelected = true; }
	void OnMouseExit() { isSelected = false; }

	public void SetType(int typeNum)
	{
		
	}
}
