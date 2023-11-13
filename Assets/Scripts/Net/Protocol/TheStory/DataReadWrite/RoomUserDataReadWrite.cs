using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace TheStory
{
	public static class RoomUserDataReadWrite
	{
		public static void WriteRoomUserData(this NetworkWriter writer, RoomUserData value)
		{
			writer.Write(value.connectId);
			writer.Write(value.characterId);
			writer.Write(value.name);
			writer.Write(value.ifSure);
			writer.Write((short)value.skills.Count);
			for (int i = 0; i < value.skills.Count; ++i)
				writer.WriteBuffDetile(value.skills[i]);
			writer.Write((short)value.tags.Count);
			for (int i = 0; i < value.tags.Count; ++i)
				writer.Write(value.tags[i]);
		}
		public static RoomUserData ReadRoomUserData(this NetworkReader reader)
		{
			RoomUserData value = new RoomUserData();
			value.connectId = reader.ReadInt();
			value.characterId = reader.ReadInt();
			value.name = reader.ReadString();
			value.ifSure = reader.ReadBool();
			value.skills = new List<BuffDetile>();
			short skillsCount = reader.ReadShort();
			for (int i = 0; i < skillsCount; ++i)
				value.skills.Add(reader.ReadBuffDetile());
			value.tags = new List<int>();
			short tagsCount = reader.ReadShort();
			for (int i = 0; i < tagsCount; ++i)
				value.tags.Add(reader.ReadInt());
			return value;
		}
	}
}