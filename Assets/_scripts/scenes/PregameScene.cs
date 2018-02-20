using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class PregameScene : ESingletonMono<PregameScene>
{



    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Pregame")).transform.SetParent(this.gameObject.transform);
    }
}
