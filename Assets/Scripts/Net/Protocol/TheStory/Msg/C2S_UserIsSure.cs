using System;
using System.Collections.Generic;
using Mirror;
using System.Text;
namespace TheStory
{
	public struct C2S_UserIsSure : NetworkMessage
	{
		public int id;
		public bool isSure;
	}
}