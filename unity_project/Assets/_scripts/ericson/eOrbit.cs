using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ericson{
	public class eOrbit : MonoBehaviour {

		public float angle = 0;
		public float speed = 100f;
		public float zOffSet = -0.5f;
		public float radius = 2;

		public bool xAxis, yAxis, zAxis;
		public bool invert = false;
		public bool activeRotation = true;
		public bool objInPlace = false;

		public Vector3 startRotation = new Vector3(0,0,225);
		

		
		void Start()
		{
			transform.rotation = Quaternion.Euler(startRotation.x, startRotation.y, startRotation.z + angle);
		}

		void Update () 
		{
			if(activeRotation) Active_Orbit();		
			else if(!objInPlace) 
			{
				Stationary_Orbit();
			}
		}
		void Active_Orbit() 
		{
			if(!invert) angle += speed * Time.deltaTime;
			else angle -= speed * Time.deltaTime;
			if(angle >= 360) angle = 0;
			float dx = GetDistanceX();
			float dy = GetDistanceY();
			SetPosition(dx, dy);
			transform.Rotate(new Vector3(0,0, (speed * Time.deltaTime)));
		}
		void Stationary_Orbit()
		{
			float dx = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			float dy = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			SetPosition(dx, dy);
			objInPlace = true;
		}
		float GetDistanceX()
		{
			return Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
		}
		float GetDistanceY()
		{
			return Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
		}
		void SetPosition(float dx, float dy)
		{
			if(zAxis) transform.localPosition = new Vector3(dx, dy, zOffSet);
			else if(yAxis) transform.localPosition = new Vector3(dx, 0, dy);
			else if(xAxis) transform.localPosition = new Vector3(0, dx, dy);
		}
	}
}