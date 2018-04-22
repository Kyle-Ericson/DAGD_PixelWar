using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;
using UnityEngine.UI;
using TMPro;

public class LobbyScene : eSingletonMono<LobbyScene>  {

	public GameObject ui = null;
	public Button findGame = null;
	public Button createGame = null;
	public Button joinGame = null;
	public TMP_InputField inputField;


	public override void Init()
	{
		ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Lobby"));
		ui.transform.SetParent(this.transform);
		findGame = ui.transform.GetChild(1).GetComponent<Button>();
		findGame.onClick.AddListener(HandleFindGame);
		createGame = ui.transform.GetChild(2).GetComponent<Button>();
		createGame.onClick.AddListener(HandleCreateGame);
		joinGame = ui.transform.GetChild(3).GetComponent<Button>();
		joinGame.onClick.AddListener(HandleJoinGame);
		inputField = ui.transform.GetChild(5).GetComponent<TMP_InputField>();
	}
	public void HandleFindGame()
	{
		SocketManager.ins.Send(PacketFactory.BuildFindGame());
		PersistentSettings.isHost = false;
		SceneManager.ins.ToWait();
	}
	public void HandleCreateGame()
	{
		PersistentSettings.isHost = true;
		SceneManager.ins.ToPregame();
	} 
	public void HandleJoinGame()
	{
		if(inputField.text == "" || inputField.text == string.Empty)
		{
			Debug.Log("Please input a game Key");
			return;
		}
		PersistentSettings.isHost = false;
		SocketManager.ins.Send(PacketFactory.BuildJoinGame(inputField.text.ToUpper()));
	}
}
