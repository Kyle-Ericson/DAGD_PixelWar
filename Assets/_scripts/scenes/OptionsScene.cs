using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;
using UnityEngine.UI;

public class OptionsScene : eSingletonMono<OptionsScene>
{


    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Options")).transform.SetParent(this.gameObject.transform);
    }


}
