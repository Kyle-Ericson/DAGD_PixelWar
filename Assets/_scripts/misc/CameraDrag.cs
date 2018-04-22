using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {
    
    private float holdTimer = 0;
    private float holdTimerMax = 0.06f;
    private Vector2[] touchpos = new Vector2[2];
    private float prevDistance = 0;
    


    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            // do touch controls here
            if(Input.touchCount == 2)
            {
                for(int i = 0; i < Input.touchCount; i++)
                {
                    if(Input.touches[i].phase == TouchPhase.Began) 
                    {
                        touchpos[i] = Input.touches[i].position;
                    }
                    else if(Input.touches[i].phase == TouchPhase.Moved)
                    {
                        touchpos[i] = Input.touches[i].position;
                    }
                }
                if(prevDistance == 0) prevDistance = GetDistance(touchpos[0], touchpos[1]);
                else
                {
                    float direction = Normalize(GetDistance(touchpos[0], touchpos[1]) - prevDistance);
                    Camera.main.orthographicSize += -direction * PersistentSettings.zoomSpeed * Time.deltaTime;
                    ClampZoom();
                    prevDistance = GetDistance(touchpos[0], touchpos[1]);
                }
            }
            else prevDistance = 0;
        }
        else 
        {
            var scrollDir = Input.GetAxis("Mouse ScrollWheel");
            Camera.main.orthographicSize += -scrollDir * PersistentSettings.zoomSpeed * Time.deltaTime;
            ClampZoom();

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
            newpos.x = Input.GetAxisRaw("Mouse X");
            newpos.y = Input.GetAxisRaw("Mouse Y");
            transform.position -= newpos * PersistentSettings.dragSpeed * Time.deltaTime;
        }
        
    }
    private float GetDistance(Vector2 posA, Vector2 posB)
    {
        return Vector2.Distance(posA, posB);
    }
    private int Normalize(float n)
    {
        return n < 0 ? -1 : n > 0 ? 1 : 0;
    }
    private int Normalize(int n)
    {
        return n < 0 ? -1 : n > 0 ? 1 : 0;
    }
    private void ClampZoom()
    {
        if (Camera.main.orthographicSize < PersistentSettings.maxZoom) Camera.main.orthographicSize = PersistentSettings.maxZoom;
        else if (Camera.main.orthographicSize > PersistentSettings.minZoom) Camera.main.orthographicSize = PersistentSettings.minZoom;
    }
}
