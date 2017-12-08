using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{

    private float zOffset = -1;
    private Vector2 _gridPosition;
    public Vector2 gridPosition
    {
        get { return _gridPosition; }
    }

    private void Start() { }

    public void Move(Vector3 newPos)
    {
        transform.position = new Vector3(newPos.x, newPos.y, zOffset);
        _gridPosition = MapLoader.instance.WorldToGrid(transform.position);
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
