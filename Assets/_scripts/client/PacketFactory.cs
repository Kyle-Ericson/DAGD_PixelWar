using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public static class PacketFactory 
{

	public static byte[] BuildJoinRequest()
	{
		JoinRequest packet = new JoinRequest(); // new join request
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
	public static byte[] BuildNewGame(int mapID)
	{
		NewGame packet = new NewGame();
		packet.mapID = mapID;
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
	public static byte[] BuildEndTurn()
	{
		EndTurn packet = new EndTurn();
		packet.type = "ENDT";
		foreach(KeyValuePair<Vector2, Unit> u in MapManager.ins.unitGrid)
		{
			UnitStats unit = new UnitStats();
			unit.unitType = (int)u.Value.type;
			unit.position = eSerializableVector2.FromVector2(u.Value.gridpos);
			unit.health = u.Value.health;
			unit.team = (int)u.Value.team;
			packet.units.Add(unit);
		}
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
}

// this is used to grab the type from the packets
[System.Serializable]
public class JsonType
{
	public string type;
}

////////////////////// packets for sending
[System.Serializable]
public class JoinRequest
{
	public string type = "JREQ";
	public string request = "Hello.";
}
[System.Serializable]
public class NewGame
{
	public string type = "NGAM";
	public int mapID;

}
[System.Serializable]
public class JoinGame
{
	public string type = "JGAM";
	public string gameKey;
}
[System.Serializable]
public class FindGame
{
	public string type = "FGAM";
}

////////////////////// packets for receiving
[System.Serializable]
public class JoinResponse
{
	public string type;
	public int response; // 0: Deny 1: Accept
}
[System.Serializable]
public class GameStart
{
	public string type;
	public int playerNumber;
	public int mapID;
}

////////////////////// packets for sending and receiving
[System.Serializable]
public class EndTurn 
{
	public string type;
	public List<UnitStats> units;
}


///////////////////// Data Structures
[System.Serializable]
public class UnitStats 
{
	public int unitType;
	public eSerializableVector2 position;
	public int team;
	public int health;
}