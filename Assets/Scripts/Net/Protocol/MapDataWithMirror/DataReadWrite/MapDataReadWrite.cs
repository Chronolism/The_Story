using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace MapDataWithMirror
{
	public static class MapDataReadWrite
	{
		public static void WriteMapData(this NetworkWriter writer, MapData value)
		{
			writer.Write(value.ID);
			writer.Write(value.name);
			writer.Write(value.description);
			writer.Write(value.maxFeather);
			writer.Write(value.maxTool);
			writer.Write((short)value.mapDetiles.Count);
			for (int i = 0; i < value.mapDetiles.Count; ++i)
				writer.WriteMapDetile(value.mapDetiles[i]);
			writer.Write((short)value.playerSpwnPos.Count);
			for (int i = 0; i < value.playerSpwnPos.Count; ++i)
				writer.WriteV2(value.playerSpwnPos[i]);
			writer.Write((short)value.servitorSpwnPos.Count);
			for (int i = 0; i < value.servitorSpwnPos.Count; ++i)
				writer.WriteV2(value.servitorSpwnPos[i]);
			writer.Write((short)value.FeatherSpwnPos.Count);
			for (int i = 0; i < value.FeatherSpwnPos.Count; ++i)
				writer.WriteV2(value.FeatherSpwnPos[i]);
			writer.Write((short)value.ToolSpwnPos.Count);
			for (int i = 0; i < value.ToolSpwnPos.Count; ++i)
				writer.WriteV2(value.ToolSpwnPos[i]);
		}
		public static MapData ReadMapData(this NetworkReader reader)
		{
			MapData value = new MapData();
			value.ID = reader.ReadInt();
			value.name = reader.ReadString();
			value.description = reader.ReadString();
			value.maxFeather = reader.ReadInt();
			value.maxTool = reader.ReadInt();
			value.mapDetiles = new List<MapDetile>();
			short mapDetilesCount = reader.ReadShort();
			for (int i = 0; i < mapDetilesCount; ++i)
				value.mapDetiles.Add(reader.ReadMapDetile());
			value.playerSpwnPos = new List<V2>();
			short playerSpwnPosCount = reader.ReadShort();
			for (int i = 0; i < playerSpwnPosCount; ++i)
				value.playerSpwnPos.Add(reader.ReadV2());
			value.servitorSpwnPos = new List<V2>();
			short servitorSpwnPosCount = reader.ReadShort();
			for (int i = 0; i < servitorSpwnPosCount; ++i)
				value.servitorSpwnPos.Add(reader.ReadV2());
			value.FeatherSpwnPos = new List<V2>();
			short FeatherSpwnPosCount = reader.ReadShort();
			for (int i = 0; i < FeatherSpwnPosCount; ++i)
				value.FeatherSpwnPos.Add(reader.ReadV2());
			value.ToolSpwnPos = new List<V2>();
			short ToolSpwnPosCount = reader.ReadShort();
			for (int i = 0; i < ToolSpwnPosCount; ++i)
				value.ToolSpwnPos.Add(reader.ReadV2());
			return value;
		}
	}
}