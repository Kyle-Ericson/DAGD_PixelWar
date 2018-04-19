using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;
using TMPro;

public class TransitionScene : eSingletonMono<TransitionScene> 
{

	private GameObject ui = null;
	private TextMeshProUGUI title = null;

	public override void Init()
	{
		ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Transition"));
        ui.transform.SetParent(this.gameObject.transform);

		title = ui.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
	}
	public void SetText(string newText)
	{
		title.text = newText;
	}
	
	
	
}
