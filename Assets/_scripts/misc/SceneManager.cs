using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class SceneManager : e_SingletonMono<SceneManager> {



    void Start ()
    {
        Init();
	}
    public override void Init()
    {
        InputEvents.ins.Init();
        Initialize_Scenes();
        ChangeScene(Scene.title);
    }
    void Initialize_Scenes()
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

    private void LoadScenes()
    {

    }

    public void ChangeScene(Scene new_scene)
    {
        HideAll();
        switch (new_scene)
        {
            case Scene.title:
                TitleScene.ins.Show();
                break;
            case Scene.postgame:
                PostgameScene.ins.Show();
                break;
            case Scene.pregame:
                PregameScene.ins.Show();
                break;
            case Scene.options:
                OptionsScene.ins.Show();
                break;
        }
    }
    public void HideAll()
    {
        GameScene.ins.CleanUp();
        GameScene.ins.Hide();
        TitleScene.ins.Hide();
        PregameScene.ins.Hide();
        OptionsScene.ins.Hide();
        PostgameScene.ins.Hide();
    }
    public void StartMatch(int map, int players)
    {
        HideAll();
        GameScene.ins.Show();
        GameScene.ins.SetupMatch(map, players);
    }
}