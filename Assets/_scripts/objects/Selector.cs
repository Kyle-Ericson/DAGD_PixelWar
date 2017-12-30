using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{

    private float zOffset = -1;

    private void Start() { }

    public void Move(Vector3 newPos)
    {
        transform.position = new Vector3(newPos.x, newPos.y, zOffset);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
