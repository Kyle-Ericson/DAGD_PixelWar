using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;
using TMPro;
using UnityEngine.UI;

public class PostgameScene : eSingletonMono<PostgameScene>
{

    private GameObject ui;
    public Button exitButton = null;
    public Button rematchButton = null;
    public TextMeshProUGUI winner;
    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public TextMeshProUGUI p1Gathered;
    public TextMeshProUGUI p2Gathered;
    public TextMeshProUGUI p1Spent;
    public TextMeshProUGUI p2Spent;
    public TextMeshProUGUI p1MaxArmy;
    public TextMeshProUGUI p2MaxArmy;

    public override void Init()
    {
        ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Postgame"));
        ui.transform.SetParent(this.gameObject.transform);
        winner = ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        p1Text = ui.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();
        p2Text = ui.transform.GetChild(1).GetChild(8).GetComponent<TextMeshProUGUI>();
        p1Gathered = ui.transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>();
        p2Gathered = ui.transform.GetChild(1).GetChild(9).GetComponent<TextMeshProUGUI>();
        p1Spent = ui.transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>();
        p2Spent = ui.transform.GetChild(1).GetChild(10).GetComponent<TextMeshProUGUI>();
        p1MaxArmy = ui.transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>();
        p2MaxArmy = ui.transform.GetChild(1).GetChild(11).GetComponent<TextMeshProUGUI>();

        exitButton = ui.transform.GetChild(3).GetComponent<Button>();
        rematchButton = ui.transform.GetChild(2).GetComponent<Button>();
        exitButton.onClick.AddListener(ExitMatch);
        rematchButton.onClick.AddListener(Rematch);
    }
    public void UpdateAll()
    {
        winner.text = "Player " + ((int)MatchStats.winner).ToString().ToUpper() + " Wins!";

        p1Text.text = "Red";
        p2Text.text = "Blue";

        p1Gathered.text = MatchStats.gatheredP1.ToString();
        p2Gathered.text = MatchStats.gatheredP2.ToString();

        p1Spent.text = MatchStats.spentP1.ToString();
        p2Spent.text = MatchStats.spentP2.ToString();

        p1MaxArmy.text = MatchStats.maxArmyValueP1.ToString();
        p2MaxArmy.text = MatchStats.maxArmyValueP2.ToString();
    }

    public void Rematch()
    {
        if(PersistentSettings.gameMode == GameMode.local) SceneManager.ins.StartMatch(MatchStats.mapID);
        else SceneManager.ins.ToTitle();
    }
    public void ExitMatch()
    {
        SceneManager.ins.ToTitle();
    }

}
