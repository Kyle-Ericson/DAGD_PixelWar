using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_Lerp : MonoBehaviour {

    public Vector3 pos1;
    public Vector3 pos2;
    private Vector3 start;
    private Vector3 target;
    private bool move = false;
    private float current_lerp_time = 0;
    public float max_lerp_time = 1f;
    public delegate void e_LerpComplete();
    public event e_LerpComplete On_e_Lerp_Complete;
    public bool in_position = false;

    private void Start()
    {
        transform.position = pos1;
    }
    void Update()
    {
        if(move)
        {
            current_lerp_time += Time.deltaTime;
            var t = current_lerp_time / max_lerp_time;
            if(t >= 1)
            {
                Stop();
            }
            transform.localPosition = Vector3.Lerp(start, target, Smoother_Step(t));
        }
    }

    private void Start_Lerp()
    {
        if (transform.localPosition == target) return;
        move = true;
    }
    public void Stop()
    {
        move = false;
        current_lerp_time = 0;
        if (On_e_Lerp_Complete != null) On_e_Lerp_Complete();
    }
    public void LerpForward()
    {
        start = pos1;
        target = pos2;
        Start_Lerp();
    }
    public void LerpBackward()
    {
        start = pos2;
        target = pos1;
        Start_Lerp();
    }
    public void Set_Positions(Vector3 new_pos1, Vector3 new_pos2)
    {
        pos1 = new_pos1;
        pos2 = new_pos2;
        current_lerp_time = 0;
    }
    
    public float Inverse_Smooth_Step(float t)
    {
        return 4 * (t * t * t) - 6 * (t * t) + 3 * t;
    }
    public float Smoother_Step(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }
}
