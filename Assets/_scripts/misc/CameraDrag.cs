using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

    public float dragSpeed = 10;
    private bool drag = false;

    void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        Vector3 newpos = Vector3.zero;

        newpos.x = Input.GetAxis("Mouse X");
        newpos.y = Input.GetAxis("Mouse Y");

        transform.position -= newpos * dragSpeed * Time.deltaTime;
    }
    public void DragCam(Vector3 mousepos)
    {
        drag = true;
    }
}
