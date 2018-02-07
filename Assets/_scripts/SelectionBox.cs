using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ericson;

public class SelectionBox : MonoBehaviour
{

    private float zOffset = -1;
    private Vector2 _gridpos = Vector2.zero * -1;
    public Vector2 gridpos
    {
        get { return _gridpos; }
    }

    private void Start()
    {
        Move(Vector2.zero);
    }

    public void Move()
    {
        Vector2 gridPos = MapManager.ins.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(gridPos != _gridpos) DoMove(gridPos);
    }
    public void Move(Vector2 gridPos)
    {
        DoMove(gridPos);
    }
    private void DoMove(Vector2 gridPos)
    {
        if (MapManager.ins.currentMap.ContainsKey(gridPos))
        {
            var newPos = MapManager.ins.GridToWorld(gridPos);
            newPos.z = zOffset;
            transform.position = newPos;
            UpdateGridPos();
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void UpdateGridPos()
    {
        _gridpos = MapManager.ins.WorldToGrid(transform.position);
        
    }
}
