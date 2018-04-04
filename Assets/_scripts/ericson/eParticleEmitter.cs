using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
	public class eParticleEmitter : MonoBehaviour 
	{
		public int numberOfParticles = 10;
		public GameObject particlePrefab = null;
		public bool loopEmission = false;
		public float interval = 1f;
		public float trackedTime = 0;
		
		public void Update()
		{
			if(loopEmission)
			{
				trackedTime += Time.deltaTime;
				if(trackedTime >= interval)
				{
					EmitParticles();
					trackedTime = 0;
				}
			}
		}


		public void EmitParticles()
		{
			for(int i = 0; i < numberOfParticles; i++)
			{
				Instantiate(particlePrefab, transform.position, Quaternion.identity);
			}
		}
	}

}
