using Mirror;
namespace TheStory
{
	public static class S2C_UserIsSureBroadcastHandler
	{
		public static void MsgHandle(S2C_UserIsSureBroadcast msg)
		{
			DataMgr.Instance.roomData.UserisSure(msg.name, msg.isSure);
        }
	}
}
