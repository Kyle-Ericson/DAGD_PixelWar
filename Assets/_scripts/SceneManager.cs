using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class SceneManager : ESingletonMono<SceneManager> {
    
     private Texture2D pointerTexture = null;
	
	void Start ()
    {
        InitializeScenes();
        pointerTexture = Resources.Load<Texture2D>("sprites/pointer");
        Cursor.visible = true;
        
       
	}

    void InitializeScenes()
    {
        GameScene.ins.gameObject.transform.SetParent(this.gameObject.transform);
        GameScene.ins.Init();
        GameScene.ins.SetupMatch(0, 2);
        Cursor.SetCursor(pointerTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void ChangeScene()
    {
        
    }
}
