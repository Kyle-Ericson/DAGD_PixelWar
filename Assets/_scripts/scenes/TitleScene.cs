using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class TitleScene : eSingletonMono<TitleScene>
{
    private GameObject ui = null;
    private Button startButton = null;
    private Button learnButton = null;
    private Button optionsButton = null;
    private Button quitButton = null;


    public override void Init()
    {
        ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Title"));
        ui.transform.SetParent(this.gameObject.transform);

        startButton = ui.transform.GetChild(1).GetComponent<Button>();
        learnButton = ui.transform.GetChild(2).GetComponent<Button>();
        optionsButton = ui.transform.GetChild(3).GetComponent<Button>();
        quitButton = ui.transform.GetChild(4).GetComponent<Button>();

        startButton.onClick.AddListener(HandleStart);
        learnButton.onClick.AddListener(HandleLearn);
        optionsButton.onClick.AddListener(HandleOptions);
        quitButton.onClick.AddListener(HandleQuit);
    }

    public void HandleStart()
    {
        SceneManager.ins.ChangeScene(Scene.pregame);
        //SceneManager.ins.Start_Match(0, 2);
    }
    public void HandleLearn()
    {
        SceneManager.ins.StartTutorial();
    }
    public void HandleOptions()
    {
        SceneManager.ins.ChangeScene(Scene.options);
    }
    public void HandleQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
