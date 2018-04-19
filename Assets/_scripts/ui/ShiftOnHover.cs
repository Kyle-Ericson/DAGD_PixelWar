using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShiftOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public int shiftAmount = 80;
    private Vector3 startpos;
    
	
	void Awake()
    {
        //if (transform.localPosition != startpos) startpos = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        //if (transform.localPosition != startpos) startpos = transform.localPosition;
        //var tempstartpos = startpos;
        //tempstartpos.x += shiftAmount;
        //transform.localPosition = tempstartpos;
        transform.localScale *= 1.5f;
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
        transform.localScale = new Vector3(1,1,1);
    }

}
