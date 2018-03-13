﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InfoBox : MonoBehaviour {

    public e_Lerp elerp = null;
    public TextMeshProUGUI text;
    public Image sprite = null;
	
	
	
	
	void Update () {
		
	}

    public void Show(Unit unit)
    {
        elerp.LerpForward();
        sprite.sprite = unit.unitIcon.sprite;
        text.text =
            unit.data.name + "\n\n"
            + "Cost: " + unit.data.cost + "\n"
            + "Attack: " + unit.data.attack + "\n"
            + "Range: " + unit.data.range + "\n"
            + "Speed: " + unit.data.speed + "\n"
            + "Health: " + unit.data.maxHealth + "\n";
    }

    public void Hide()
    {
        elerp.LerpBackward();
    }
}