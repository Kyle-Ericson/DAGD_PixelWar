using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Ericson
{

    public class EMenu : MonoBehaviour
    {

        private GameObject buttonPrefab = null;
        private List<Button> buttons = new List<Button>();

        private void Start()
        {
            buttonPrefab = Resources.Load<GameObject>("prefabs/EButton");
            gameObject.SetActive(false);
        }

        public void Clear()
        {
            if (buttons.Count > 0)
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
            if (buttonPrefab == null) buttonPrefab = Resources.Load<GameObject>("prefabs/EButton");
            float buttonH = buttonPrefab.GetComponent<RectTransform>().rect.height;
            var panelRT = GetComponent<RectTransform>();
            panelRT.sizeDelta = new Vector2(200, buttons.Count * buttonH);
            float menuH = panelRT.rect.height;

            for (int i = 0; i < buttons.Count; i++)
            {
                RectTransform rT = buttons[i].GetComponent<RectTransform>();


                rT.offsetMin = new Vector2(0, ((menuH / 2) - (buttonH / 2)) - (buttonH * i) - 20);
                rT.offsetMax = new Vector2(0, rT.offsetMin.y + buttonH);
            }
        }

        public Button AddButton(string label)
        {
            Button newButton = Instantiate(buttonPrefab).GetComponent<Button>();
            newButton.gameObject.transform.SetParent(gameObject.transform);
            newButton.transform.localScale = Vector3.one;
            newButton.transform.localPosition = Vector3.zero;
            newButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = label;
            buttons.Add(newButton);
            UpdateMenu();
            return newButton;
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }


    }
}