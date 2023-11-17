using Mirror;
namespace TheStory
{
	public static class C2S_QuitRoomHandler
	{
		public static void MsgHandle(NetworkConnectionToClient con, C2S_QuitRoom msg ,int channelId)
		{
			if(con.address == "localhost") { return; }
			DataMgr.Instance.roomData.RemoveUser(msg.name, con);
		}
	}
}
