using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShiftOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public int shiftAmount = 80;
    
	
	void Start ()
    {
    
	}

    public void OnPointerEnter(PointerEventData data)
    {
        var start_pos = transform.position;
        start_pos.x += shiftAmount;
        transform.position = start_pos;
    }
    public void OnPointerExit(PointerEventData data)
    {
        var start_pos = transform.position;
        start_pos.x -= shiftAmount;
        transform.position = start_pos;
    }
}
