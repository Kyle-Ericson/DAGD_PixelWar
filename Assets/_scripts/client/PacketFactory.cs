using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public static class PacketFactory 
{

	public static byte[] BuildJoinRequest()
	{
		JoinRequest packet = new JoinRequest(); // new join request
		packet.type = "JREQ";
		packet.request = "Hello World";
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
	public static byte[] BuildNewGame(int mapID, bool isPrivate)
	{
		NewGame packet = new NewGame();
		packet.type = "NGAM";
		packet.mapID = mapID;
		packet.isPrivate = isPrivate;
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
	public static byte[] BuildEndTurn()
	{
		EndTurn packet = new EndTurn();
		packet.type = "ENDT";
		packet.gameKey = PersistentSettings.gameKey;
		packet.units = new List<UnitStats>();
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
	public static byte[] BuildFindGame()
	{
		FindGame packet = new FindGame();
		packet.type = "FGAM";
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
	public static byte[] BuildGameStart()
	{
		GameStart packet = new GameStart();
		packet.type = "GMST";
		return System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(packet));
	}
	public static byte[] BuildJoinGame(string key)
	{
		JoinGame packet = new JoinGame();
		packet.type = "JGAM";
		packet.gameKey = key;
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
public class JoinRequest // JREQ
{
	public string type;
	public string request;
}
[System.Serializable]
public class NewGame // NGAM
{
	public string type;
	public int mapID;
	public bool isPrivate;
}
[System.Serializable]
public class JoinGame // JGAM
{
	public string type;
	public string gameKey;
}
[System.Serializable]
public class FindGame // FGAM
{
	public string type;
}
////////////////////// packets for receiving
[System.Serializable]
public class JoinResponse // JRES
{
	public string type;
	public int response; // 0: Deny 1: Accept
}
////////////////////// packets for sending and receiving
[System.Serializable]
public class EndTurn // ENDT
{
	public string type;
	public List<UnitStats> units;
	public string gameKey;
}
[System.Serializable]
public class GameStart // GMST
{
	public string type;
	public int mapID;
	public string gameKey;
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