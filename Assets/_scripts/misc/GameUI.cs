using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ericson;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Button endTurn;
    public ELerp transition1 = null;
    public ELerp transition2 = null;
    public TextMeshProUGUI playerText = null;
    public TextMeshProUGUI turnText = null;
    private int transitionCount = 0;
    public Image turnBarImage = null;
    public TextMeshProUGUI turnBarText = null;
    public Image armyValueImage = null;
    public TextMeshProUGUI armyValueText = null;

    private void Start()
    {
        endTurn.onClick.AddListener(BeginTurnTransition);
    }
    public void BeginTurnTransition()
    {
        GameScene.ins.AllFogOn();
        GameScene.ins.AllUnitsOff();
        GameScene.ins.StateAnimating();
        GameScene.ins.NextTurn();
        playerText.text = "Player " + GameScene.ins.currentTurn.ToString();
        turnText.text = "Turn " + GameScene.ins.turnCount.ToString();
        transition1.OnLerpComplete += CheckTransition;
        transition2.OnLerpComplete += CheckTransition;
        transition1.BeginLerp();
        transition2.BeginLerp();
    }
    private void CheckTransition()
    {
        transitionCount++;
        if (transitionCount >= 2)
        {
            GameScene.ins.EndTurn();
            turnBarText.text = turnText.text;
            UpdateArmyText();
            transitionCount = 0;
            transition1.OnLerpComplete -= CheckTransition;
            transition2.OnLerpComplete -= CheckTransition;
        }
    }
    public void ShowEndButton()
    {
        endTurn.gameObject.SetActive(true);
    }
    public void HideEndButton()
    {
        endTurn.gameObject.SetActive(false);
    }
    public void UpdateArmyText()
    {
        armyValueText.text = "Army Value: " + GameScene.ins.GetArmyValue();
    }
}
