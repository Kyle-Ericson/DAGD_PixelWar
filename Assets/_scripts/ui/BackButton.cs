using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButton : MonoBehaviour {

    private Button button;
    public bool isOptions = false;


    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }
    private void HandleClick()
    {

        if(!isOptions) SceneManager.ins.ToTitle();
        else SceneManager.ins.ToggleOptions();
    }

}
