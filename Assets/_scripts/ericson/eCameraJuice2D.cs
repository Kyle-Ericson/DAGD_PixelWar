using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
    public class eCameraJuice2D: MonoBehaviour
    {
        private bool dirty = false;
        private float friction = 1f;
        private float juice = 0f;
        private float shake { get { return juice * juice; } }
        private float maxAngle = 10f;
        private float maxOffset = 0.5f;
        Vector3 startpos;
        Quaternion startrot;


        void Update()
        {
            if(juice != 0)
            {
                if(!dirty)
                {
                    StorePosition();
                    StoreRotation();
                    dirty = true;
                }
                ApplyStress();
                ApplyFriction();
            }
        }
        public void AddJuice(float value)
        {
            juice += value;
            juice = Mathf.Clamp01(juice);
        }
        private void ApplyStress()
        {
            var seed = Random.Range(-100f, 100f);
            float offsetX = maxOffset * shake * GetNoise(seed); // x offset
            float offsetY = maxOffset * shake * GetNoise(seed + 1); // y offset
            float angle = maxAngle * shake * GetNoise(seed + 2); // angle
            transform.position = startpos + new Vector3(offsetX, offsetY, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, startrot.z + angle));
        }
        private void ApplyFriction()
        {
            juice -= friction * Time.deltaTime; // remove trauma over time
            if (juice <= 0)
            {
                juice = 0;
                dirty = false;
            }
        }
        private float GetNoise(float seed)
        {
            return Mathf.PerlinNoise(seed, seed) * Time.timeScale * Random.Range(-1, 1);
        }
        private void StorePosition()
        {
            startpos = transform.position;
        }
        private void StoreRotation()
        {
            startrot = transform.rotation;
        }
    }
}
