using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Button endTurn;
    public Button surrenderButton;
    public Button optionsButton;
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
    public GameObject tutorialPanel = null;
    public TextMeshProUGUI tutorialText = null;
    public eLerp inGameMenu = null;
    public Button menuButton = null;
    private bool gameOver = false;
    public InfoBox infobox = null;
    public bool menuOpen = false;

    private void Start()
    {
        endTurn.onClick.AddListener(BeginTurnTransition);
        menuButton.onClick.AddListener(OpenMenu);
        surrenderButton.onClick.AddListener(Surrender);
        optionsButton.onClick.AddListener(HandleOptions);
    }
    public void BeginTurnTransition()
    {
        GameScene.ins.AllFogOn();
        GameScene.ins.AllUnitsOff();
        GameScene.ins.StateAnimating();
        GameScene.ins.NextTurn();
        playerText.text = "Player " + GameScene.ins.currentTurn.ToString();
        turnText.text = "Turn " + GameScene.ins.turnCount.ToString();
        gameOver = false;
        transition1Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        transition2Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        turnBarImage.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];

        foodCountText.text = "Money:";
        armyValueText.text = "Army Value:";
        transition1.maxLerpTime = 2f;
        transition2.maxLerpTime = 2f;
        transition1.OnLerpComplete += CheckTransition;
        transition2.OnLerpComplete += CheckTransition;
        transition1.BeginLerp();
        transition2.BeginLerp();
    }
    public void BeginEndGameTransition()
    {
        GameScene.ins.StateAnimating();
        playerText.text = "Player " + GameScene.ins.currentTurn.ToString();
        turnText.text =  "Wins!";
        gameOver = true;

        transition1Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        transition2Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        turnBarImage.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        transition1.maxLerpTime = 4f;
        transition2.maxLerpTime = 4f;
        transition1.OnLerpComplete += CheckTransition;
        transition2.OnLerpComplete += CheckTransition;
        transition1.BeginHalfLerp();
        transition2.BeginHalfLerp();
    }
    private void CheckTransition()
    {
        transitionCount++;
        if (transitionCount >= 2)
        {
            if(gameOver)
            {
                SceneManager.ins.ChangeScene(Scene.postgame);
            }
            GameScene.ins.EndTurn();
            turnBarText.text = turnText.text;
            UpdateArmyText();
            UpdateFoodText();
            transitionCount = 0;
            transition1.OnLerpComplete -= CheckTransition;
            transition2.OnLerpComplete -= CheckTransition;
        }
    }
    public void OpenMenu()
    {
        inGameMenu.LerpForward();
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(CloseMenu);
        menuButton.transform.Rotate(0,0,180);
        menuOpen = true;
    }
    public void CloseMenu()
    {
        menuOpen = false;
        inGameMenu.LerpBackward();
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(OpenMenu);
        menuButton.transform.Rotate(0,0,-180);
    }
    public void HandleOptions()
    {
        SceneManager.ins.ToggleOptions();
    }
    public void ResetTransitions()
    {
        transition1Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        transition2Image.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        turnBarImage.color = PersistentSettings.teamColors[(Team)GameScene.ins.currentTurn];
        transition1.Reset();
        transition2.Reset();
        
    }
    private void Surrender()
    {
        GameScene.ins.Surrender();
    }
    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }
    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }
    public void UpdateTutorial()
    {
        tutorialText.text = Database.tutorialData[(int)GameScene.ins.tutorialPhase];
    }
    public void UpdateTutorial(string text)
    {
        tutorialText.text = text;
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
        foodCountText.text = "Money: " + GameScene.ins.GetTeamFoodCount();
    }
    public void ShowInfo(Unit unit)
    {
        infobox.Show(unit);
    }
    public void HideInfo()
    {
        infobox.Hide();
    }
    public void CleanUp()
    {
        ResetTransitions();
    }
}
