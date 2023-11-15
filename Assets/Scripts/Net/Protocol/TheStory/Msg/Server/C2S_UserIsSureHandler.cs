using Mirror;
namespace TheStory
{
	public static class C2S_UserIsSureHandler
	{
		public static void MsgHandle(NetworkConnectionToClient con, C2S_UserIsSure msg ,int channelId)
		{
			S2C_UserIsSureBroadcast broadcastMsg = new S2C_UserIsSureBroadcast();
			//在下面编辑消息处理内容;
			if(!DataMgr.Instance.roomData.UserisSure(msg.name, msg.isSure))
			{
				return;
			}
			broadcastMsg.name = msg.name;
			broadcastMsg.isSure = msg.isSure;
			NetworkServer.SendToAll(broadcastMsg);;
		}
	}
}
