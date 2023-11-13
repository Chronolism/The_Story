using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace TheStory
{
	public class PlayerData
	{
		public int id;
		public float atk;
		public bool sex;
		public long lev;
		public int[] arrays;
		public List<int> list;
		public Dictionary<int, string> dic;
	}

	public static class PlayerDataReadWrite
	{
		public static void WritePlayerData(this NetworkWriter writer, PlayerData value)
		{
			writer.Write(value.id);
			writer.Write(value.atk);
			writer.Write(value.sex);
			writer.Write(value.lev);
			writer.Write((short)value.arrays.Length);
			for (int i = 0; i < value.arrays.Length; ++i)
				writer.Write(value.arrays[i]);
			writer.Write((short)value.list.Count);
			for (int i = 0; i < value.list.Count; ++i)
				writer.Write(value.list[i]);
			writer.Write((short)value.dic.Count);
			foreach (int key in value.dic.Keys)
			{
				writer.Write(key);
				writer.Write(value.dic[key]);
			}
		}
		public static PlayerData ReadPlayerData(this NetworkReader reader)
		{
			PlayerData value = new PlayerData();
			value.id = reader.ReadInt();
			value.atk = reader.ReadFloat();
			value.sex = reader.ReadBool();
			value.lev = reader.ReadLong();
			short arraysLength = reader.ReadShort();
			value.arrays = new int[arraysLength];
			for (int i = 0; i < value.arrays.Length; ++i)
				value.arrays[i] = reader.ReadInt();
			value.list = new List<int>();
			short listCount = reader.ReadShort();
			for (int i = 0; i < listCount; ++i)
				value.list.Add(reader.ReadInt());
			value.dic = new Dictionary<int, string>();
			short dicCount = reader.ReadShort();
			for (int i = 0; i < dicCount; ++i)
				value.dic.Add(reader.ReadInt(), reader.ReadString());
			return value;
		}
	}
}