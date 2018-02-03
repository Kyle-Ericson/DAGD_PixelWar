using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ericson;

public class GameUI : MonoBehaviour
{
    public Button endTurn;

    private void Start()
    {

        endTurn.onClick.AddListener(GameScene.ins.EndTurn);
    }
}
