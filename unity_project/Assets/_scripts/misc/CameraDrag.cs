using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {
    
    private float holdTimer = 0;
    private float holdTimerMax = 0.06f;
    private Vector2[] touchpos = new Vector2[2];
    private float prevDistance = 0;
    private float dragThreshold = 1f;
    private Vector3 panTranslate = Vector3.zero;
    


    void Update()
    {
        if(!GameScene.ins.running) return;
        if(Application.platform == RuntimePlatform.Android) TouchControl();
        else MouseControl();
    }
    private void TouchControl()
    {
        if(Input.touchCount == 2) TouchZoom();
        else if(Input.touchCount == 3) TouchDrag();
        else prevDistance = 0;

        ClampDrag();
        ClampZoom();
    }
    private void TouchZoom()
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
        else if(Mathf.Abs(GetDistance(touchpos[0], touchpos[1]) - prevDistance) > dragThreshold)
        {
            float direction = GetSign(GetDistance(touchpos[0], touchpos[1]) - prevDistance) * Time.deltaTime;
            Camera.main.orthographicSize += -direction * PersistentSettings.zoomSpeed * Time.deltaTime;
            prevDistance = GetDistance(touchpos[0], touchpos[1]);
        }
    }
    private void TouchDrag()
    {
        var totalMove = Vector3.zero;
        var averageMove = Vector3.zero;
        foreach (Touch thisTouch in Input.touches) 
        {
            totalMove.x += thisTouch.deltaPosition.x;
            totalMove.y += thisTouch.deltaPosition.y;
        }
        averageMove.x = (totalMove.x / Input.touchCount) * Time.deltaTime;
        averageMove.y = (totalMove.y / Input.touchCount) * Time.deltaTime;
        transform.position -= averageMove * PersistentSettings.dragSpeed * Time.deltaTime;
        
    }
    private void MouseControl()
    {
        MouseZoom();
        MouseDrag();
        ClampZoom();
        ClampDrag();
    }
    private void MouseDrag()
    {
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
    private void MouseZoom()
    {
        var scrollDir = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += -scrollDir * PersistentSettings.zoomSpeed * Time.deltaTime;
    }
    private float GetDistance(Vector2 posA, Vector2 posB)
    {
        return Vector2.Distance(posA, posB);
    }
    private int GetSign(float n)
    {
        return n < 0 ? -1 : n > 0 ? 1 : 0;
    }
    private int GetSign(int n)
    {
        return n < 0 ? -1 : n > 0 ? 1 : 0;
    }
    private Vector2 CalcMidPoint(Vector2 vectorA, Vector2 vectorB)
    {
        return Vector2.Lerp(vectorA, vectorB, 0.5f);
    }
    private void ClampZoom()
    {
        if (Camera.main.orthographicSize < PersistentSettings.maxZoom) Camera.main.orthographicSize = PersistentSettings.maxZoom;
        else if (Camera.main.orthographicSize > PersistentSettings.minZoom) Camera.main.orthographicSize = PersistentSettings.minZoom;
    }
    private void ClampDrag()
    {
        Vector3 clamp = transform.position;
        if(transform.position.x > MapManager.ins.currentMapData.cols - 1) clamp.x = MapManager.ins.currentMapData.cols - 1;
        else if(transform.position.x < 0) clamp.x = 0;
        if(transform.position.y < -(MapManager.ins.currentMapData.rows - 1)) clamp.y = -(MapManager.ins.currentMapData.rows - 1);
        else if(transform.position.y > 0) clamp.y = 0;
        transform.position = clamp;
    }
}
