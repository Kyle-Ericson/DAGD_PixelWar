using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ericson
{

    public class e_Menu : MonoBehaviour
    {

        private GameObject basicButtonFab = null;
        private GameObject radialButtonFab = null;
        private List<Button> buttons = new List<Button>();
        private List<Button> radialButtons = new List<Button>();

        private void Start()
        {
            basicButtonFab = Resources.Load<GameObject>("prefabs/BasicButton");
            radialButtonFab = Resources.Load<GameObject>("prefabs/RadialButton");
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
            if (radialButtons.Count > 0)
            {

                for (int i = radialButtons.Count - 1; i >= 0; i--)
                {
                    Destroy(radialButtons[i].gameObject);

                }
                radialButtons.Clear();
            }
        }
        private void UpdateMenu()
        {
            float buttonH = basicButtonFab.GetComponent<RectTransform>().rect.height;
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
        public void UpdateRadialMenu()
        {
            var newAngle = 360 / radialButtons.Count;
            for (int i = 0; i < radialButtons.Count; i++)
            {
                e_Orbit orbit = radialButtons[i].gameObject.GetComponent<e_Orbit>();
                orbit.activeRotation = false;
                orbit.angle = i * newAngle;
            }
        }
        public Button AddBasicButton(string label)
        {
            if (basicButtonFab == null) basicButtonFab = Resources.Load<GameObject>("prefabs/EButton");
            Button newButton = Instantiate(basicButtonFab).GetComponent<Button>();
            newButton.GetComponent<RadialButton>().Hide();
            newButton.gameObject.transform.SetParent(gameObject.transform);
            newButton.transform.localScale = Vector3.one;
            newButton.transform.localPosition = Vector3.zero;
            newButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = label;
            buttons.Add(newButton);
            return newButton;
        }
        public Button AddRadialButton(Sprite sprite)
        {
            if (radialButtonFab == null) radialButtonFab = Resources.Load<GameObject>("prefabs/RadialButton");
            Button newButton = Instantiate(radialButtonFab).GetComponent<Button>();
            newButton.transform.position = gameObject.transform.position;
            newButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            newButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.black;
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
        public Button AddRadialWait()
        {
            return AddRadialButton(Sprites.ins.iconSprites[Icon.wait]);
        }
        public Button AddRadialEat()
        {
            return AddRadialButton(Sprites.ins.iconSprites[Icon.eat]);
        }
        public Button AddRadialSplit()
        {
            return AddRadialButton(Sprites.ins.iconSprites[Icon.split]);
        }
        public Button AddRadialAttack()
        {
            return AddRadialButton(Sprites.ins.iconSprites[Icon.attack]);
        }
        public Button AddRadialTank()
        {
            return AddRadialButton(Sprites.ins.unitSprites[UnitType.tank]);
        }
        public Button AddRadialInfantry()
        {
            return AddRadialButton(Sprites.ins.unitSprites[UnitType.infantry]);
        }
        public Button AddRadialScout()
        {
            return AddRadialButton(Sprites.ins.unitSprites[UnitType.scout]);
        }
        public Button AddRadialSniper()
        {
            return AddRadialButton(Sprites.ins.unitSprites[UnitType.sniper]);
        }
        public Button AddRadialOriginal()
        {
            return AddRadialButton(Sprites.ins.unitSprites[UnitType.queen]);
        }



    }
}