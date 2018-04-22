using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingAction : GameState {

	public override void HandleLeftClick()
	{
		
	}
	public override void HandleRightClick()
	{
		GameScene.ins.Undo();
	}
}
