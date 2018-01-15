using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour {

    private GameObject buttonPrefab = null;
    private GameObject canvas = null;
    private List<Button> buttons = new List<Button>();

    private void Start()
    {        

    }

    public void Clear()
    {
        if(buttons.Count > 0)
        {

            for (int i = buttons.Count - 1; i >= 0; i--)
            {
                Destroy(buttons[i].gameObject);

            }
            buttons.Clear();
        }
    }

    private void UpdateMenu()
    {
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        
        for (int i = 0; i < buttons.Count; i++)
        {
            float buttonHeight = buttons[i].gameObject.GetComponent<RectTransform>().rect.height;
            //var newPos = new Vector3(0,  ((-i * (buttonHeight / 2)) - 5) + (canvasHeight / 2) , 0);
            var newPos = new Vector3(0, (5 * (buttons.Count - 1)) - (i * 10), 0);
            buttons[i].transform.localPosition = newPos;
        }
    }

    public Button AddButton(string label)
    {
        if(buttonPrefab == null) buttonPrefab = Resources.Load<GameObject>("prefabs/ActionButton");
        if(canvas == null) canvas = transform.GetChild(0).gameObject;
        Button button = Instantiate(buttonPrefab).GetComponent<Button>();
        RectTransform rectT = button.GetComponent<RectTransform>();      
        button.transform.SetParent(canvas.transform);
        button.transform.localPosition = Vector3.zero;
        rectT.offsetMin = new Vector2(0, 0);
        rectT.offsetMax = new Vector2(0, 0);
        rectT.sizeDelta = new Vector2(0, 20);
        button.transform.localScale = new Vector3(1, 0.5f, 1);
        button.GetComponentInChildren<Text>().text = label;
        buttons.Add(button);
        UpdateMenu();
        return button;
    }


}
