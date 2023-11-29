using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace MapDataWithMirror
{
	public static class MapTileDetileValueReadWrite
	{
		public static void WriteMapTileDetileValue(this NetworkWriter writer, MapTileDetileValue value)
		{
			writer.Write((short)value.value.Count);
			for (int i = 0; i < value.value.Count; ++i)
				writer.Write(value.value[i]);
		}
		public static MapTileDetileValue ReadMapTileDetileValue(this NetworkReader reader)
		{
			MapTileDetileValue value = new MapTileDetileValue();
			value.value = new List<float>();
			short valueCount = reader.ReadShort();
			for (int i = 0; i < valueCount; ++i)
				value.value.Add(reader.ReadFloat());
			return value;
		}
	}
}