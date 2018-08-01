using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingAttack : GameState {

	public override void HandleLeftClick()
	{
		if (GameScene.ins.currentSelected.inAttackRange.Contains(GameScene.ins.selectionbox.gridpos)
        && MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos))
		{
			GameScene.ins.BeginAttack();
			if(GameScene.ins.currentSelected.type != UnitType.worker) 
			{
				GameScene.ins.gettingAttacked = MapManager.ins.unitGrid[GameScene.ins.selectionbox.gridpos];
			}
			else GameScene.ins.gettingAttacked = null;
		}
	}
	public override void HandleRightClick()
	{
		GameScene.ins.Undo();
	}
}
