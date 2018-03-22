using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class SelectionBox : MonoBehaviour
{

    private float zOffset = -1f;
    public Vector2 _gridpos = Vector2.zero;
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
        if (!MapManager.ins.currentMap.ContainsKey(gridPos))
        {
            Hide();
            CheckInfoBox();
            return;
        }
        else
        {
            Show();
            DoMove(gridPos);
        }
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
            CheckInfoBox();
        }
        
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        _gridpos = new Vector2(-1, -1);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void UpdateGridPos()
    {
        _gridpos = MapManager.ins.WorldToGrid(transform.position);
        
    }
    private void CheckInfoBox()
    {
        if (MapManager.ins.unitGrid.ContainsKey(gridpos) && MapManager.ins.unitGrid[gridpos].team == (Team)GameScene.ins.currentTurn )
        {
            GameScene.ins.gameUI.ShowInfo(MapManager.ins.unitGrid[gridpos]);
        }
        else GameScene.ins.gameUI.HideInfo();
    }
}
