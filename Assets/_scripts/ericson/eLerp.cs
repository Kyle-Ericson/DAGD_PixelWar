using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eLerp : MonoBehaviour {

    public Vector3 pos1;
    public Vector3 pos2;
    private Vector3 start;
    private Vector3 target;
    private bool move = false;
    private float currentLerpTime = 0;
    public float maxLerpTime = 1f;
    public delegate void eLerpComplete();
    public event eLerpComplete OnLerpComplete;
    public bool inPosition = false;

    private void Start()
    {
        pos1 = transform.localPosition;
    }
    void Update()
    {
        if(move)
        {
            
            currentLerpTime += Time.deltaTime;
            var t = currentLerpTime / maxLerpTime;
            if(t >= 1)
            {
                Stop();
                return;
            }
            transform.localPosition = Vector3.Lerp(start, target, SmootherStep(t));
        }
    }

    private void StartLerp()
    {
        move = true;
    }
    public void Stop()
    {
        move = false;
        currentLerpTime = 0;
        transform.localPosition = target;
        if (OnLerpComplete != null) OnLerpComplete();
    }
    public void LerpForward()
    {
        start = pos1;
        target = pos2;
        if (transform.localPosition == target) return;
        StartLerp();
    }
    public void LerpBackward()
    {
        start = pos2;
        target = pos1;
        if (transform.localPosition == target) return;
        StartLerp();
    }
    public void SetPositions(Vector3 newpos1, Vector3 newpos2)
    {
        pos1 = newpos1;
        pos2 = newpos2;
        currentLerpTime = 0;
    }
    
    public float InverseSmoothStep(float t)
    {
        return 4 * (t * t * t) - 6 * (t * t) + 3 * t;
    }
    public float SmootherStep(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }
}
