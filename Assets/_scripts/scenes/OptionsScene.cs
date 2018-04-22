using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;
using UnityEngine.UI;
using TMPro;

public class OptionsScene : eSingletonMono<OptionsScene>
{
    private GameObject ui = null;

    private GameObject sfxVolume = null;
    private Slider sfxSlider = null;
    private TextMeshProUGUI sfxValueText = null;

    private GameObject musicVolume = null;
    private Slider musicSlider = null;
    private TextMeshProUGUI musicValueText = null;

    private GameObject creditsPanel = null;
    private Button creditsButton = null;


    public override void Init()
    {
        ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Options"));
        ui.transform.SetParent(this.gameObject.transform);

        sfxVolume = ui.transform.GetChild(4).gameObject;
        sfxSlider = sfxVolume.transform.GetChild(1).GetComponent<Slider>();
        sfxValueText = sfxVolume.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        sfxSlider.onValueChanged.AddListener(HandleSFXSliderChange);

        musicVolume = ui.transform.GetChild(3).gameObject;
        musicSlider = musicVolume.transform.GetChild(1).GetComponent<Slider>();
        musicValueText = musicVolume.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        musicSlider.onValueChanged.AddListener(HandleMusicSliderChange);

        creditsButton = ui.transform.GetChild(7).GetComponent<Button>();
        creditsButton.onClick.AddListener(ToggleCredits);

        creditsPanel = ui.transform.GetChild(6).gameObject;
    }
    public void HandleSFXSliderChange(float value)
    {
        PersistentSettings.SetSFXVolume(value);
        sfxValueText.text = ((int)(value * 100f)).ToString();
    }
    private void HandleMusicSliderChange(float value)
    {
        PersistentSettings.SetMusicVolume(value);
        musicValueText.text = ((int)(value * 100f)).ToString();
    }
    public void ToggleCredits()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
    public bool CreditsOpen()
    {
        return creditsPanel.activeSelf;
    }
}
