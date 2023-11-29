using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace MapDataWithMirror
{
	public static class MapTileDetileReadWrite
	{
		public static void WriteMapTileDetile(this NetworkWriter writer, MapTileDetile value)
		{
			writer.Write(value.id);
			writer.Write(value.ifHaveValue);
		}
		public static MapTileDetile ReadMapTileDetile(this NetworkReader reader)
		{
			MapTileDetile value = new MapTileDetile();
			value.id = reader.ReadInt();
			value.ifHaveValue = reader.ReadBool();
			return value;
		}
	}
}