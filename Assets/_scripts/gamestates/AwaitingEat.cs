using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingEat : GameState {

	public override void HandleLeftClick()
	{
		if(GameScene.ins.currentSelected.foodInRange.Contains(GameScene.ins.selectionbox.gridpos)) 
		{
			GameScene.ins.BeginAttack();
		}
	}
	public override void HandleRightClick()
	{
		GameScene.ins.Undo();
	}
}
