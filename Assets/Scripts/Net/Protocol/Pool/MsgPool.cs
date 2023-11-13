using System;
using System.Collections.Generic;
using Mirror;
using TheStory;
public class MsgPool
{
	public MsgPool()
	{
		NetworkServer.RegisterHandler<C2S_JionRoom>(C2S_JionRoomHandler.MsgHandle);
	}
}
