using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PostUI : MonoBehaviour {

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





    public void UpdateAll()
    {
        winner.text = MatchStats.winner.ToString().ToUpper() + " Wins!";

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
        SceneManager.ins.StartMatch(MatchStats.mapID, 2);
    }
    public void ExitMatch()
    {
        SceneManager.ins.ChangeScene(Scene.title);
    }
}
