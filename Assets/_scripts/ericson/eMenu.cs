using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ericson
{

    public class eMenu : MonoBehaviour
    {

        private GameObject basicButtonFab = null;
        private GameObject radialButtonFab = null;
        private List<Button> buttons = new List<Button>();
        private List<Button> radialButtons = new List<Button>();
        private int buttonGap = 10;

        private void Start()
        {
            basicButtonFab = Resources.Load<GameObject>("prefabs/BasicButton");
            radialButtonFab = Resources.Load<GameObject>("prefabs/RadialButton");
            //gameObject.SetActive(false);
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
            if (radialButtons.Count > 0)
            {

                for (int i = radialButtons.Count - 1; i >= 0; i--)
                {
                    Destroy(radialButtons[i].gameObject);

                }
                radialButtons.Clear();
            }
        }
        public void UpdateMenu()
        {
            float buttonH = basicButtonFab.GetComponent<RectTransform>().rect.height;
            var panelRT = GetComponent<RectTransform>();
            panelRT.sizeDelta = new Vector2(400, buttons.Count * (buttonH + buttonGap));
            float menuH = panelRT.rect.height;

            for (int i = 0; i < buttons.Count; i++)
            {
                RectTransform rT = buttons[i].GetComponent<RectTransform>();
                rT.offsetMin = new Vector2(-panelRT.rect.width / 2, -(buttonH / 2));
                rT.offsetMax = new Vector2(panelRT.rect.width / 2, (buttonH / 2));
                rT.transform.localPosition = new Vector3(rT.rect.width * 0.5f, -((buttonH + buttonGap) * i));
                
            }
        }
        public void UpdateRadialMenu()
        {
            var newAngle = 360 / radialButtons.Count;
            for (int i = 0; i < radialButtons.Count; i++)
            {
                eOrbit orbit = radialButtons[i].gameObject.GetComponent<eOrbit>();
                orbit.activeRotation = false;
                orbit.angle = i * newAngle;
                var text = radialButtons[i].transform.GetChild(1);
                text.transform.Rotate(0,0, ((i * newAngle) - 135) * -1);
                
            }
        }
        public Button AddBasicButton(string label)
        {
            if (basicButtonFab == null) basicButtonFab = Resources.Load<GameObject>("prefabs/BasicButton");
            Button newButton = Instantiate(basicButtonFab).GetComponent<Button>();
            newButton.gameObject.transform.SetParent(gameObject.transform);
            newButton.transform.localScale = Vector3.one;
            newButton.transform.localPosition = Vector3.zero;
            newButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = label;
            buttons.Add(newButton);
            return newButton;
        }
        public Button AddRadialButton(UnitType type)
        {
            if (radialButtonFab == null) radialButtonFab = Resources.Load<GameObject>("prefabs/RadialButton");
            Button newButton = Instantiate(radialButtonFab).GetComponent<Button>();
            newButton.transform.position = gameObject.transform.position;
            newButton.transform.GetChild(0).gameObject.SetActive(true);
            newButton.transform.GetChild(1).gameObject.SetActive(false);
            newButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = Sprites.ins.unitsSprites[type];
            newButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            newButton.gameObject.transform.SetParent(gameObject.transform);
            if(type == UnitType.tank) newButton.transform.GetChild(0).transform.localScale = new Vector3(0.75f, 0.75f, 1);
            radialButtons.Add(newButton);
            return newButton;
        }
        public Button AddRadialButton(Sprite sprite)
        {
            if (radialButtonFab == null) radialButtonFab = Resources.Load<GameObject>("prefabs/RadialButton");
            Button newButton = Instantiate(radialButtonFab).GetComponent<Button>();
            newButton.transform.position = gameObject.transform.position;
            newButton.transform.GetChild(0).gameObject.SetActive(true);
            newButton.transform.GetChild(1).gameObject.SetActive(false);
            newButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            newButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            newButton.gameObject.transform.SetParent(gameObject.transform);
            radialButtons.Add(newButton);
            return newButton;
        }
        public Button AddRadialButton(string text)
        {
            if (radialButtonFab == null) radialButtonFab = Resources.Load<GameObject>("prefabs/RadialButton");
            Button newButton = Instantiate(radialButtonFab).GetComponent<Button>();
            newButton.transform.position = gameObject.transform.position;
            newButton.transform.GetChild(0).gameObject.SetActive(false);
            newButton.transform.GetChild(1).gameObject.SetActive(true);
            newButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = text;
            newButton.gameObject.transform.SetParent(gameObject.transform);
            radialButtons.Add(newButton);
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
        public Button AddRadialUnit(UnitType type)
        {
            return AddRadialButton(type);
        }



    }
}