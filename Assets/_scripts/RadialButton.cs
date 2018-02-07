using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour {

	private SpriteRenderer icon = null;
	private Sprite background = null;


	void Start()
	{
		Hide();
		icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
		background = Resources.Load<Sprite>("sprites/ui/tag_background");
		GetComponent<Image>().sprite = background;
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
	public void Hide()
	{
		var newpos = transform.position;
		newpos.z = 2;
		transform.position = newpos;
	}
	public void Show()
	{
		var newpos = transform.position;
		newpos.z = -0.01f;
		transform.position = newpos;
	}
}
