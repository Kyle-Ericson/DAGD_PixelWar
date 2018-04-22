using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingSplit : GameState {

	public override void HandleLeftClick()
	{
		if(GameScene.ins.currentSelected.inSplitRange.Contains(GameScene.ins.selectionbox.gridpos) 
		&& !MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos))
		{
			GameScene.ins.ConfirmSplit(GameScene.ins.selectionbox.gridpos);
		}
		else if(MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos))
		{

		}
	}
	public override void HandleRightClick()
	{
		GameScene.ins.Undo();
	}
}
