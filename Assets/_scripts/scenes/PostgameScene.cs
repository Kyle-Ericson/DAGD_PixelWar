using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class PostgameScene : eSingletonMono<PostgameScene>
{


    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Postgame")).transform.SetParent(this.gameObject.transform);
    }

}
