using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {
    
    private float holdTimer = 0;
    private float holdTimerMax = 0.06f;

    void Update()
    {
        var scrollDir = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += -scrollDir * PersistentSettings.zoomSpeed * Time.deltaTime;
        if (Camera.main.orthographicSize < PersistentSettings.maxZoom) Camera.main.orthographicSize = PersistentSettings.maxZoom;
        else if (Camera.main.orthographicSize > PersistentSettings.minZoom) Camera.main.orthographicSize = PersistentSettings.minZoom;

        if (!Input.GetMouseButton(2))
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

        transform.position -= newpos * PersistentSettings.dragSpeed * Time.deltaTime;
       


    }
}
