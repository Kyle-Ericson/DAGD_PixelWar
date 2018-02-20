using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class TitleScene : ESingletonMono<TitleScene>
{


    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Title")).transform.SetParent(this.gameObject.transform);
    }
}
