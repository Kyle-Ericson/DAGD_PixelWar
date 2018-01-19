using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class SceneManager : ESingletonMono<SceneManager> {
    
     
	
	void Start ()
    {
        InitializeScenes();
	}

    void InitializeScenes()
    {
        GameScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
        GameScene.ins.Init();
        GameScene.ins.SetupMatch(1, 2);
    }
    public void ChangeScene()
    {
        
    }
}
