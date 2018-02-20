using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class PostgameScene : ESingletonMono<PostgameScene>
{


    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Postgame")).transform.SetParent(this.gameObject.transform);
    }

}
