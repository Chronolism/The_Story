using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace MapDataWithMirror
{
	public static class V2ReadWrite
	{
		public static void WriteV2(this NetworkWriter writer, V2 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
		}
		public static V2 ReadV2(this NetworkReader reader)
		{
			V2 value = new V2();
			value.x = reader.ReadInt();
			value.y = reader.ReadInt();
			return value;
		}
	}
}