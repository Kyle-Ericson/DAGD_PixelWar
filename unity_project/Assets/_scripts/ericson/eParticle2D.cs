using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ericson
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class eParticle2D : MonoBehaviour 
	{
		private SpriteRenderer spriteRenderer = null;
		private Vector3 direction = Vector3.zero;
		private float lifeTime = 0;
		private float speed = 0;
		
		public Sprite sprite = null;
		public float maxLifeTime = 3f;
		public float maxSpeed = 5f;
		public float minSpeed = 2f;



		public void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = this.sprite;
			direction.x = Random.Range(-1f, 1f);
			direction.y = Random.Range(-1f, 1f);
			lifeTime = maxLifeTime;
			speed = Random.Range(minSpeed, maxSpeed);
		}

		public void Update()
		{
			lifeTime -= Time.deltaTime;
			transform.position += direction * speed * Time.deltaTime;

			var newColor = spriteRenderer.color;
			newColor.a = lifeTime / maxLifeTime;
			spriteRenderer.color = newColor;

			if(lifeTime <= 0) Destroy(this.gameObject);
		}
	}

}