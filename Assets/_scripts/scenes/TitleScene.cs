using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class TitleScene : eSingletonMono<TitleScene>
{
    private GameObject ui = null;
    private Button offlineButton = null;
    private Button onlineButton = null;
    private Button learnButton = null;
    private Button optionsButton = null;
    private Button quitButton = null;
 


    public override void Init()
    {
        ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Title"));
        ui.transform.SetParent(this.gameObject.transform);

        offlineButton = ui.transform.GetChild(1).GetComponent<Button>();
        offlineButton.onClick.AddListener(HandleOffline);

        onlineButton = ui.transform.GetChild(2).GetComponent<Button>();
        onlineButton.onClick.AddListener(HandleOnline);

        learnButton = ui.transform.GetChild(3).GetComponent<Button>();
        learnButton.onClick.AddListener(HandleLearn);

        optionsButton = ui.transform.GetChild(4).GetComponent<Button>();
        optionsButton.onClick.AddListener(HandleOptions);

        quitButton = ui.transform.GetChild(5).GetComponent<Button>();
        quitButton.onClick.AddListener(HandleQuit);
    }
    public void HandleOnline()
    {
        SocketManager.ins.ConnectLocal();
        //SocketManager.ins.ConnectOnline();
        PersistentSettings.gameMode = GameMode.online;
        SceneManager.ins.ToLobby();
    }
    public void HandleOffline()
    {
        PersistentSettings.gameMode = GameMode.local;
        SceneManager.ins.ToPregame();
    }
    public void HandleLearn()
    {
        PersistentSettings.gameMode = GameMode.tutorial;
        SceneManager.ins.StartMatch(0);
    }
    public void HandleOptions()
    {
        SceneManager.ins.ToggleOptions();
    }
    public void HandleQuit()
    {
        Application.Quit();
    }
}
