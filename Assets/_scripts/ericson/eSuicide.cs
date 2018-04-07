using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eSuicide : MonoBehaviour {

	public float lifetime = 1f;
	
	void Update () 
	{
		lifetime -= Time.deltaTime;
		if(lifetime <= 0) Destroy(this.gameObject);
	}
}
