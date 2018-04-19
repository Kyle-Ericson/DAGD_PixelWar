using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using ericson;

public class SocketManager : eSingletonMono<SocketManager> {


	private UdpClient client = null;
	private IPEndPoint serverEndPoint;
	private IPAddress serverIP;
	string localhost = "127.0.0.1";
	string ec2_HelloWorld = "18.188.129.215";
	int serverPort = 5643;
	int clientPort = 5646;
	List<string> packetBuffer = new List<string>();

	public void Update()
	{
		if(packetBuffer.Count > 0)
		{
			ReadFromBuffer();
		}
	}
	private void ReadFromBuffer()
	{
		lock(packetBuffer)
		{
			string nextPacket = packetBuffer[0];
			JsonType typeChecker = JsonUtility.FromJson<JsonType>(nextPacket);
			switch(typeChecker.type)
			{
				case "JRES":
					PacketHandler.HandleJoinResponse(nextPacket);
					break;
				case "ENDT":
					PacketHandler.HandleEndTurn(nextPacket);
					break;
				case "GMST":
					PacketHandler.HandleGameStart(nextPacket);
					break;
			}
			packetBuffer.Remove(nextPacket);
		}
	}
	public void ConnectLocal() 
	{
		if(client != null && client.Client.Connected) return;
		serverEndPoint = new IPEndPoint(IPAddress.Parse(localhost), serverPort);
		client = new UdpClient(clientPort);
		client.Connect(serverEndPoint);
		client.Client.Blocking = false;
        client.BeginReceive(new AsyncCallback(Receive), client);

		if(client.Client.Connected) Debug.Log("Client Connected! :)");
		else Debug.Log("Connection Failed. :(");
	}
	public void ConnectOnline()
	{
		if(NotOnline()) return;
		if(client != null && client.Client.Connected) return;
		serverEndPoint = new IPEndPoint(IPAddress.Parse(ec2_HelloWorld), serverPort);
		client = new UdpClient(5644);
		client.Connect(serverEndPoint);
		client.Client.Blocking = false;
        client.BeginReceive(new AsyncCallback(Receive), client);

		if(client.Client.Connected) Debug.Log("Client Connected! :)");
		else Debug.Log("Connection Failed. :(");
	}
	public void Send(byte[] dgram)
	{
		if(client != null && client.Client.Connected) 
		{
			client.Send(dgram, dgram.Length);
		}
		else Debug.Log("Client not connected. :(");
	}
	public void Receive(IAsyncResult result)
	{
		try
		{
			byte [] received = client.EndReceive(result, ref serverEndPoint);
			string packet = System.Text.Encoding.UTF8.GetString(received);
			JsonType json = JsonUtility.FromJson<JsonType>(packet);
			if(json.type != null) AddToBuffer(packet);
			client.BeginReceive(new AsyncCallback(Receive), client);
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
			throw e;
		}
	}
	public void Disconnect()
	{
		if(client != null) 
		{
			client.Close();
			client = null;
		}
	}
	public bool NotOnline() 
	{
		return (Application.internetReachability == NetworkReachability.NotReachable);
	}
	private void AddToBuffer(string packet)
	{
		Debug.Log("Join Response");
		packetBuffer.Add(packet);
	}
}
