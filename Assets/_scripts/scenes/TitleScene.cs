using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class TitleScene : e_SingletonMono<TitleScene>
{
    private GameObject title_ui = null;
    private Button bttn_start = null;
    private Button bttn_options = null;
    private Button bttn_quit = null;


    public override void Init()
    {
        title_ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Title"));
        title_ui.transform.SetParent(this.gameObject.transform);

        bttn_start = title_ui.transform.GetChild(1).GetComponent<Button>();
        bttn_options = title_ui.transform.GetChild(2).GetComponent<Button>();
        bttn_quit = title_ui.transform.GetChild(3).GetComponent<Button>();

        bttn_start.onClick.AddListener(Handle_Start);
        bttn_options.onClick.AddListener(Handle_Options);
        bttn_quit.onClick.AddListener(Handle_Quit);
    }

    public void Handle_Start()
    {
        //Scene_Manager.ins.Change_Scene(Scene.pregame);
        SceneManager.ins.Start_Match(0, 2);
    }
    public void Handle_Options()
    {
        SceneManager.ins.Change_Scene(Scene.options);
    }
    public void Handle_Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
