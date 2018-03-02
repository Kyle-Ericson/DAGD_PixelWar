using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

    public float dragSpeed = 5;
    private float holdTimer = 0;
    private float holdTimerMax = 0.06f;

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            holdTimer = 0;
            return;
        }
        else
        {
            holdTimer += Time.deltaTime;
            if (holdTimer < holdTimerMax) return;
        }


        Vector3 newpos = Vector3.zero;

        newpos.x = Input.GetAxis("Mouse X");
        newpos.y = Input.GetAxis("Mouse Y");

        transform.position -= newpos * dragSpeed * Time.deltaTime;
    }
}
