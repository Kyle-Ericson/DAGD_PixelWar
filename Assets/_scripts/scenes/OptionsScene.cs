using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class OptionsScene : ESingletonMono<OptionsScene>
{


    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Options")).transform.SetParent(this.gameObject.transform);
    }


}
