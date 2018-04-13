using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeOption : MonoBehaviour {

    private Slider slider = null;
    private TextMeshProUGUI valueText = null;


    public void Start()
    {
        slider = transform.GetChild(1).GetComponent<Slider>();
        valueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(HandleSliderChange);
    }
    public void HandleSliderChange(float value)
    {
        PersistentSettings.SetVolume(value);
        valueText.text = ((int)(value * 100f)).ToString();
    }

}
