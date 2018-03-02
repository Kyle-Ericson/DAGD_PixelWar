using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class PregameScene : e_SingletonMono<PregameScene>
{
    public GameObject ui = null;


    public override void Init()
    {
        ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Pregame"));
        ui.transform.SetParent(this.gameObject.transform);
    }
}
