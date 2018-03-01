using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class OptionsScene : e_SingletonMono<OptionsScene>
{


    public override void Init()
    {
        Instantiate(Resources.Load<GameObject>("prefabs/scenes/Options")).transform.SetParent(this.gameObject.transform);
    }


}
