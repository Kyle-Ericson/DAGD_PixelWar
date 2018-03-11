using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
    public class TransitionLerp : MonoBehaviour
    {


        public bool doLerp = false;
        public float maxLerpTime = 1.0f;
        private float currentLerpTime = 0f;
        private Vector3 start;
        public Vector3 target;
        public delegate void LerpComplete();
        public event LerpComplete OnLerpComplete;
        public float offset = 150f;


        private void Awake()
        {
            start = transform.localPosition;
            target = start;
            target.x = (-target.x);
            if (start.x > 0) target.x += offset;
            else if (start.x < 0) target.x -= offset;
        }
        void Update()
        {

            if (doLerp)
            {
                currentLerpTime += Time.deltaTime;
                var t = currentLerpTime / maxLerpTime;
                if (t > 1)
                {
                    Stop();
                    return;
                }
                transform.localPosition = Vector3.Lerp(start, target, Inverse_Smooth_Step(t));
            }

        }
        public void BeginLerp()
        {
            doLerp = true;
        }
        public void Stop()
        {
            doLerp = false;
            transform.localPosition = start;
            currentLerpTime = 0;
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