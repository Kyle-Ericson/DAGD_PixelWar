using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;
using TMPro;

public class TransitionScene : eSingletonMono<TransitionScene> 
{

	private GameObject ui = null;
	private TextMeshProUGUI title = null;
	private Button backButton = null;

	public override void Init()
	{
		ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Transition"));
        ui.transform.SetParent(this.gameObject.transform);

		title = ui.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
		backButton = ui.transform.GetChild(1).GetComponent<Button>();
		backButton.onClick.AddListener(HandleBack);
	}
	public void SetText(string newText)
	{
		title.text = newText;
	}
	public void HandleBack() 
	{
		if(PersistentSettings.gameMode == GameMode.online && !GameScene.ins.running) {
            SocketManager.ins.Send(PacketFactory.BuildLeave());            
        }
	}
	
	
	
}
