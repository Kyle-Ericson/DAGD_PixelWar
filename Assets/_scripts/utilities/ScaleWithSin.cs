using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithSin : MonoBehaviour {


    public float theta = 0;
    public float amplitude = 0.05f;
    public float speed = 1;
    public float baseLine = 1;
    public Vector3 newScale; 
	
	void Update ()
    {
        theta += Time.deltaTime * speed;
        newScale.x = newScale.y = (Mathf.Sin(theta) * amplitude) + baseLine;
        gameObject.transform.localScale = newScale;
	}
}
