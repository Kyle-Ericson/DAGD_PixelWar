using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunWithSine : MonoBehaviour {


    public float theta = 0;
    public float amplitude = 0.05f;
    public float speed = 1;
    public float baseLine = 1;
    public Vector3 newScale;
    public bool doScale = false;
    public bool doAlpha = false;
    public bool startRandom = false;
	
    void Start()
    {
        if(startRandom) theta = Random.Range(0f, 360f);
    }
	void Update ()
    {
        
        theta += Time.deltaTime * speed;
        if (doScale) DoScale(theta);
        if (doAlpha) DoAlpha(theta);
	}
    private void DoScale(float _theta)
    {
        newScale.x = newScale.y = (Mathf.Sin(_theta) * amplitude) + baseLine;
        gameObject.transform.localScale = newScale;
    }
    private void DoAlpha(float _theta)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        Color newColor = sr.color;
        newColor.a = (Mathf.Sin(theta) * amplitude) + baseLine;
        sr.color = newColor;
    }
}
