using Mirror;
namespace TheStory
{
	public static class C2S_JionRoomHandler
	{
		public static void MsgHandle(NetworkConnectionToClient con, C2S_JionRoom msg ,int channelId)
		{
            DataMgr.Instance.roomData.AddRoomUser(msg.name, con);
        }
	}
}
