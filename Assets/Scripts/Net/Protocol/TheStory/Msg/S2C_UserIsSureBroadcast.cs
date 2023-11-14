using System;
using System.Collections.Generic;
using Mirror;
using System.Text;
namespace TheStory
{
	public struct S2C_UserIsSureBroadcast : NetworkMessage
	{
		public int id;
		public bool isSure;
	}
}