using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingInput : GameState {

	public override void HandleLeftClick()
	{
		if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos) &&
			MapManager.ins.unitGrid[GameScene.ins.selectionbox.gridpos].state != UnitState.sleeping &&
			MapManager.ins.unitGrid[GameScene.ins.selectionbox.gridpos].team == (Team)GameScene.ins.currentTurn)
		{
			GameScene.ins.SelectUnit();
			
		}
		else
		{
			GameScene.ins.DeselectAllTiles();
		}
	}
	public override void HandleRightClick()
	{
		GameScene.ins.DeselectAllTiles();
		if (MapManager.ins.unitGrid.ContainsKey(GameScene.ins.selectionbox.gridpos))
		{
			var unit = MapManager.ins.unitGrid[GameScene.ins.selectionbox.gridpos];
			unit.CheckAttackRange();

			foreach(Vector2 v in unit.inAttackRange)
			{
				if (v == GameScene.ins.selectionbox.gridpos) continue;
				Tile tile = MapManager.ins.currentMap[v];
				tile.Highlight();
				tile.SetIconColor(Color.red);
			}
		}
	}
	
}
