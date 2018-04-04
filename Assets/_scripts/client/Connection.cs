using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using ericson;

public class Connection {

	TcpClient client;
	NetworkStream stream;
	string localhost = "127.0.0.1";
	string ec2_HelloWorld = "ec2-18-188-129-215.us-east-2.compute.amazonaws.com";

	public void ConnectLocal() {
		client = new TcpClient(localhost, 5643);
		stream = client.GetStream();
	}
	public void ConnectOnline()
	{
		client = new TcpClient(ec2_HelloWorld, 5643);
		stream = client.GetStream();
	}
	public void Close()
	{
		client.Close();
	}
	public void Send()
	{
		byte [] packet = PacketFactory.buildJoinRequest();
		if(client.Connected) stream.Write(packet, 0, packet.Length);
		stream.Flush();
	}

	
	
}
