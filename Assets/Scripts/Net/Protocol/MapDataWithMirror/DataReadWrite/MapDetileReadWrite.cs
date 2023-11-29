using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;

namespace MapDataWithMirror
{
	public static class MapDetileReadWrite
	{
		public static void WriteMapDetile(this NetworkWriter writer, MapDetile value)
		{
			writer.Write(value.id);
			writer.Write(value.name);
			writer.Write(value.layer);
			writer.Write((short)value.MapTileDetiles.Count);
			foreach (V2 key in value.MapTileDetiles.Keys)
			{
				writer.WriteV2(key);
				writer.WriteMapTileDetile(value.MapTileDetiles[key]);
			}
			writer.Write((short)value.tileValue.Count);
			foreach (V2 key in value.tileValue.Keys)
			{
				writer.WriteV2(key);
				writer.WriteMapTileDetileValue(value.tileValue[key]);
			}
		}
		public static MapDetile ReadMapDetile(this NetworkReader reader)
		{
			MapDetile value = new MapDetile();
			value.id = reader.ReadInt();
			value.name = reader.ReadString();
			value.layer = reader.ReadString();
			value.MapTileDetiles = new Dictionary<V2, MapTileDetile>();
			short MapTileDetilesCount = reader.ReadShort();
			for (int i = 0; i < MapTileDetilesCount; ++i)
				value.MapTileDetiles.Add(reader.ReadV2(), reader.ReadMapTileDetile());
			value.tileValue = new Dictionary<V2, MapTileDetileValue>();
			short tileValueCount = reader.ReadShort();
			for (int i = 0; i < tileValueCount; ++i)
				value.tileValue.Add(reader.ReadV2(), reader.ReadMapTileDetileValue());
			return value;
		}
	}
}