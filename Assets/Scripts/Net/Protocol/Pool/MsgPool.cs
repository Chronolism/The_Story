using System;
using System.Collections.Generic;
using Mirror;
using TheStory;
public class MsgPool
{
	public MsgPool()
	{
		NetworkServer.RegisterHandler<C2S_JionRoom>(C2S_JionRoomHandler.MsgHandle);
		NetworkServer.RegisterHandler<C2S_UserIsSure>(C2S_UserIsSureHandler.MsgHandle);
		NetworkClient.RegisterHandler<S2C_UserIsSureBroadcast>(S2C_UserIsSureBroadcastHandler.MsgHandle);
	}
}
