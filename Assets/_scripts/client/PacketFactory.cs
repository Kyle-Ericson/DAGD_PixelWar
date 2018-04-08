using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public static class PacketFactory 
{

	public static byte[] buildJoinRequest()
	{
		JoinRequest packet = new JoinRequest(); // new join request
		string jsonPacket = JsonUtility.ToJson(packet); // convert to json
		return System.Text.Encoding.ASCII.GetBytes(jsonPacket);
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



////////////////////// packets for receiving
[System.Serializable]
public class JoinResponse
{
	public string type;
	public int response; // 0: Deny 1: Accept
}
