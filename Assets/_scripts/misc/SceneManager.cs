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
        Change_Scene(Scene.title);
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

    private void Load_Scenes()
    {

    }

    public void Change_Scene(Scene new_scene)
    {
        Hide_All();
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
    public void Hide_All()
    {
        GameScene.ins.CleanUp();
        GameScene.ins.Hide();
        TitleScene.ins.Hide();
        PregameScene.ins.Hide();
        OptionsScene.ins.Hide();
        PostgameScene.ins.Hide();
    }
    public void Start_Match(int map, int players)
    {
        Hide_All();
        GameScene.ins.Show();
        GameScene.ins.SetupMatch(map, players);
    }
}