using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class SceneManager : eSingletonMono<SceneManager> {



    public bool isOptionsOpen = false;

    void Start ()
    {
        Init();
	}
    public override void Init()
    {
        InputEvents.ins.Init();
        InitializeScenes();
        ChangeScene(Scene.title);
    }
    void InitializeScenes()
    {
        GameScene.ins.Init();
        TitleScene.ins.Init();
        PregameScene.ins.Init();
        PostgameScene.ins.Init();
        OptionsScene.ins.Init();

        GameScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
        TitleScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
        PregameScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
        PostgameScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
        OptionsScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
    }


    public void ChangeScene(Scene newScene)
    {
        HideAll();
        switch (newScene)
        {
            case Scene.title:
                TitleScene.ins.Show();
                break;
            case Scene.postgame:
                PostgameScene.ins.Show();
                PostgameScene.ins.UpdateUI();
                break;
            case Scene.pregame:
                PregameScene.ins.Show();
                break;
        }
    }
    public void HideAll()
    {
        GameScene.ins.Hide();
        TitleScene.ins.Hide();
        PregameScene.ins.Hide();
        OptionsScene.ins.Hide();
        isOptionsOpen = false;
        PostgameScene.ins.Hide();
    }
    public void StartMatch(int map, int players)
    {
        HideAll();
        GameScene.ins.Show();
        GameScene.ins.SetupMatch(map, players, false);
    }
    public void StartTutorial()
    {
        HideAll();
        GameScene.ins.Show();
        GameScene.ins.SetupMatch(0, 2, true);
    }
    public void ToggleOptions()
    {
        isOptionsOpen = !isOptionsOpen;
        OptionsScene.ins.gameObject.SetActive(isOptionsOpen);
    }
}