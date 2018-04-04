using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public static class PacketFactory 
{

	public static byte[] buildJoinRequest()
	{
		eBuffer buffer = new eBuffer(); // create a new buffer
		JoinRequest packet = new JoinRequest(); // new join request
		packet.msg = "Hello."; // add a message
		string jsonPacket = JsonUtility.ToJson(packet); // convert to json

		buffer.Add("JREQ"); // add the type header
		buffer.Add(jsonPacket.Length); // add packet size to header
		buffer.Add(jsonPacket); // add json packet to buffer
		return buffer.data;

	}
}

[System.Serializable]
public class JoinRequest
{
	public string msg;
}
