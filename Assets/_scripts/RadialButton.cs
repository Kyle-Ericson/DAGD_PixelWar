using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButton : MonoBehaviour {

	private SpriteRenderer icon = null;


	void Start()
	{
		icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		var newRot = transform.rotation;
		newRot.x -= newRot.x;
		newRot.y -= newRot.y;
		newRot.z -= newRot.z;

		icon.transform.rotation = newRot;
		
	}
}
