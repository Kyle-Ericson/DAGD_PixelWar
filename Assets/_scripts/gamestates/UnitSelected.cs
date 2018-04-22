using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelected : GameState {

	public override void HandleLeftClick()
	{
		if (GameScene.ins.currentSelected.inMoveRange.Contains(GameScene.ins.selectionbox.gridpos) 
		&& !MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos)
		|| GameScene.ins.selectionbox.gridpos == GameScene.ins.currentSelected.gridpos)
		{ 
			GameScene.ins.currentSelected.StartMoving(AStar.ins.FindPath(GameScene.ins.currentSelected.gridpos, GameScene.ins.selectionbox.gridpos));
			GameScene.ins.StateAnimating();
		}
		else GameScene.ins.Undo();

	}
	public override void HandleRightClick()
	{
		GameScene.ins.Undo();
	}
}
