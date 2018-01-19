using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ericson;

public class GameUI : MonoBehaviour
{

    public EMenu actionMenu = null;
    public Button endTurn;

    private void Start()
    {
        actionMenu = transform.Find("ActionMenu").GetComponent<EMenu>();
        endTurn.onClick.AddListener(GameScene.ins.EndTurn);
    }
    public Button AddButton(string label)
    {
        return actionMenu.AddButton(label);
    }
    public void ClearActionMenu()
    {
        actionMenu.Clear();
    }
    public void ShowActionMenu()
    {
        actionMenu.Show();
    }

}
