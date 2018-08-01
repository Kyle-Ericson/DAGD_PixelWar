using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ericson
{
	public class eBuffer
	{
		private byte[] _data;
		public byte[] data { get { return _data; } }

		public eBuffer()
		{
			_data = new byte[0];
		}

		public void Add(byte[] newArr)
		{
			byte[] temp = new byte[_data.Length + newArr.Length];
			for(int i = 0; i < temp.Length; i++)
			{
				if(i < _data.Length) temp[i] = _data[i];
				else temp[i] = newArr[i - _data.Length];
			}
			_data = temp;
		}
		public void Add(byte newByte)
		{
			byte[] temp = new byte[_data.Length + 1];
			for(int i = 0; i < temp.Length; i++)
			{
				if(i < _data.Length) temp[i] = _data[i];
				else temp[i] = newByte;
			}
			_data = temp;
		}
		public void Add(string newString)
		{
			byte[] temp = System.Text.Encoding.ASCII.GetBytes(newString);
			Add(temp);
		}
		public void Add(int newInt)
		{
			byte[] temp = BitConverter.GetBytes(newInt);
			Add(temp);
		}
	}

}
