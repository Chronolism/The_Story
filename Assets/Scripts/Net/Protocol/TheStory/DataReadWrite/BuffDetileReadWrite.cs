using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace TheStory
{
	public static class BuffDetileReadWrite
	{
		public static void WriteBuffDetile(this NetworkWriter writer, BuffDetile value)
		{
			writer.Write(value.buffId);
			writer.Write(value.buffValue);
		}
		public static BuffDetile ReadBuffDetile(this NetworkReader reader)
		{
			BuffDetile value = new BuffDetile();
			value.buffId = reader.ReadInt();
			value.buffValue = reader.ReadInt();
			return value;
		}
	}
}