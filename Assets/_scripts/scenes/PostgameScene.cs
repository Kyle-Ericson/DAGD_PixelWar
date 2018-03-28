using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class PostgameScene : eSingletonMono<PostgameScene>
{

    private GameObject postUI;

    public override void Init()
    {
        postUI = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Postgame"));
        postUI.transform.SetParent(this.gameObject.transform);
        
    }
    public void UpdateUI()
    {
        postUI.GetComponent<PostUI>().UpdateAll();
    }

}
