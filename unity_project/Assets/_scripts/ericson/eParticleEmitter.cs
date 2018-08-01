using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ericson
{
	public class eParticleEmitter : MonoBehaviour 
	{
		private float trackedTime = 0;
		private float interval = 1f;

		public GameObject particlePrefab = null;
		public int numberOfParticles = 10;
		public float maxInterval = 1f;
		public float minInterval = 1f;

		public bool loopEmission = false;
		public bool randomSpawns = false;
		
		
		public void Update()
		{
			if(loopEmission)
			{
				interval = Random.Range(minInterval, maxInterval);
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
				Vector3 position = transform.position;
				if(randomSpawns) 
				{
					position = new Vector3(
					Random.Range(transform.position.x - (transform.localScale.x * 0.5f), transform.position.x + (transform.localScale.x * 0.5f)),
					Random.Range(transform.position.y - (transform.localScale.y * 0.5f), transform.position.y + (transform.localScale.y * 0.5f)),
					0);
				}
				var particle = Instantiate(particlePrefab, position, Quaternion.identity);
				particle.transform.position = position;
			}
		}
	}

}
