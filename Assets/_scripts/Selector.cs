using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

	private float zOffset = -0.02f;
	private Vector2 _gridPosition;
	public Vector2 gridPosition
	{
		get { return _gridPosition; }
		set { _gridPosition = value; }
	}

	
	public void Move(Vector2 newPos) 
	{
		transform.position = new Vector3(newPos.x, newPos.y, zOffset);
	}
}
