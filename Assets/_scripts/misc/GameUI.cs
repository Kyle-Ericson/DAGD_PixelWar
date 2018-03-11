using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Button endTurn;
    public TransitionLerp transition1 = null;
    public TransitionLerp transition2 = null;
    public Image transition1Image = null;
    public Image transition2Image = null;
    public TextMeshProUGUI playerText = null;
    public TextMeshProUGUI turnText = null;
    private int transitionCount = 0;
    public Image turnBarImage = null;
    public TextMeshProUGUI turnBarText = null;
    public TextMeshProUGUI armyValueText = null;
    public TextMeshProUGUI foodCountText = null;

    public InfoBox info_box = null;

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

        transition1Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        transition2Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        turnBarImage.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];

        foodCountText.text = "Food:";
        armyValueText.text = "Army Value:";
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
            UpdateFoodText();
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
    public void UpdateFoodText()
    {
        foodCountText.text = "Food: " + GameScene.ins.GetTeamFoodCount();
    }
    public void Show_Info(Unit unit)
    {
        info_box.Show(unit);
    }
    public void Hide_Info()
    {
        info_box.Hide();
    }
}
