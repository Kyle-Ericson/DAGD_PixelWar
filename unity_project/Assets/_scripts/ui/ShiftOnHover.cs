using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShiftOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public int shiftAmount = 80;
    private Vector3 startpos;
    private Vector3 originalScale;
    
	
	void Awake()
    {
        originalScale = transform.localScale;
        //if (transform.localPosition != startpos) startpos = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        //if (transform.localPosition != startpos) startpos = transform.localPosition;
        //var tempstartpos = startpos;
        //tempstartpos.x += shiftAmount;
        //transform.localPosition = tempstartpos;
        transform.localScale *= 1.5f;
        SoundManager.ins.PlayHoverButton();
    }
    public void OnPointerExit(PointerEventData data)
    {
        Reset();
    }
    public void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        //transform.localPosition = startpos;
        transform.localScale = originalScale;
    }

}
