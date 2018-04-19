using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;


public class InputEvents : eSingletonMono<InputEvents> {


    public Vector3 prevMousePos = Vector3.zero;
    public delegate void MouseLClick();
    public event MouseLClick OnMouseLClick;
    public delegate void MouseRClick();
    public event MouseRClick OnMouseRClick;
    public delegate void MouseMoved();
    public event MouseMoved OnMouseMoved;
    

    private void Update()
    {
        if (GameScene.ins.gameState == GameState.paused) return;
        if(GameScene.ins.running)
        {
            CheckInputs();
        }
        TestingInputs();
    }
    private void CheckInputs()
    {
        if (Input.GetMouseButtonDown(0) && OnMouseLClick != null) OnMouseLClick();
        if (Input.GetMouseButtonDown(1) && OnMouseRClick != null) OnMouseRClick();
        if (Input.mousePosition != prevMousePos && OnMouseMoved != null)
        {
            OnMouseMoved();
            prevMousePos = Input.mousePosition;
        }
    }
    private void TestingInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos)) return;
            GameScene.ins.SpawnUnit(UnitType.scout, (Team)GameScene.ins.currentTurn, GameScene.ins.selectionbox.gridpos);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos)) return;
            GameScene.ins.SpawnUnit(UnitType.soldier, (Team)GameScene.ins.currentTurn, GameScene.ins.selectionbox.gridpos);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos)) return;
            GameScene.ins.SpawnUnit(UnitType.sniper, (Team)GameScene.ins.currentTurn, GameScene.ins.selectionbox.gridpos);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos)) return;
            GameScene.ins.SpawnUnit(UnitType.tank, (Team)GameScene.ins.currentTurn, GameScene.ins.selectionbox.gridpos);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos)) return;
            GameScene.ins.SpawnUnit(UnitType.worker, (Team)GameScene.ins.currentTurn, GameScene.ins.selectionbox.gridpos);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eCameraJuice2D camjuice = Camera.main.gameObject.GetComponent<eCameraJuice2D>();
            if (camjuice) camjuice.AddJuice(0.2f);
        }
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     SocketManager.ins.ConnectOnline();
        // }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     SocketManager.ins.ConnectLocal();
        // }
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     SocketManager.ins.Send(PacketFactory.BuildJoinRequest());
        // }
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     SocketManager.ins.Disconnect();
        // }
    }
    
}
