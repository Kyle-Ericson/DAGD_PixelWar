using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
    public class Transition_Lerp : MonoBehaviour
    {


        public bool do_lerp = false;
        public float max_lerp_time = 1.0f;
        private float current_lerp_time = 0f;
        private Vector3 start_pos;
        public Vector3 target;
        public delegate void LerpComplete();
        public event LerpComplete OnLerpComplete;
        private float transition_offset = 100f;


        private void Awake()
        {
            start_pos = transform.localPosition;
            target = start_pos;
            target.x = (-target.x);
            if (start_pos.x > 0) target.x += transition_offset;
            else if (start_pos.x < 0) target.x -= transition_offset;
        }
        void Update()
        {

            if (do_lerp)
            {
                current_lerp_time += Time.deltaTime;
                var t = current_lerp_time / max_lerp_time;
                if (t > 1)
                {
                    Stop();
                    return;
                }
                transform.localPosition = Vector3.Lerp(start_pos, target, Inverse_Smooth_Step(t));
            }

        }
        public void BeginLerp()
        {
            do_lerp = true;
        }
        public void Stop()
        {
            do_lerp = false;
            transform.localPosition = start_pos;
            current_lerp_time = 0;
            if (OnLerpComplete != null)
            {
                OnLerpComplete();
            }
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
}