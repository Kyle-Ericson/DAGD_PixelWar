using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	public float angle = -45f;
	public float speed = 2f;
	public bool doFlips = false;
	
	private float timer = 0f;
	public float maxTime = 1f;
	
	// Update is called once per frame
	void Update () 
	{
		float x = Mathf.Cos(angle);
		float y = Mathf.Sin(angle);

		Vector3 velocity = new Vector3(x, y, 0) * speed * Time.deltaTime;

		transform.position += velocity;

		if(doFlips) {
			timer += Time.deltaTime;
			if(timer > maxTime) {
				GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
				timer = 0;
			}
		} 
		
	}
}
