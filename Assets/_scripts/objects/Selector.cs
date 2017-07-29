using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

	private float zOffset = 0f;
	private Vector2 _position;
	public Vector2 position	{
		get { return _position; }
		set { _position = value; }
	}

	private void Start() {
		GetComponent<SpriteRenderer>().sortingOrder = 100;
	}
	public void Move(Vector2 newPos) {
		transform.position = new Vector3(newPos.x, newPos.y, zOffset);
	}
	public void Hide() {
		gameObject.SetActive(false);
	}
	public void Show() {
		gameObject.SetActive(true);
	}
}
