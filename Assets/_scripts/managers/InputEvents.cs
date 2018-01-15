using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;


public class InputEvents : ESingletonMono<InputEvents> {


    public Vector3 prevMousePos = Vector3.zero;
    public delegate void MouseLClick();
    public event MouseLClick OnMouseLClick;
    public delegate void MouseRClick();
    public event MouseRClick OnMouseRClick;
    public delegate void MouseMoved();
    public event MouseMoved OnMouseMoved;
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) if (OnMouseLClick != null) OnMouseLClick();
        if (Input.GetMouseButtonDown(1)) if (OnMouseRClick != null) OnMouseRClick();
        if (Input.mousePosition != prevMousePos)
        {
            if (OnMouseMoved != null) OnMouseMoved();
            prevMousePos = Input.mousePosition;
        }

    }
    
}
