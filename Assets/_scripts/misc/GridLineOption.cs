using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridLineOption : MonoBehaviour {

    private Toggle toggle = null;


	void Start ()
    {
        toggle = transform.GetChild(1).GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(HandleToggleChange);
	}
    void HandleToggleChange(bool b)
    {
        PersistentSettings.SetGridLines(b);
    }
	
}
