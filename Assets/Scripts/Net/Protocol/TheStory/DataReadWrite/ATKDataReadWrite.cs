using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace TheStory
{
	public static class ATKDataReadWrite
	{
		public static void WriteATKData(this NetworkWriter writer, ATKData value)
		{
			writer.Write(value.id);
			writer.Write(value.pre);
			writer.Write(value.value);
			writer.Write(value.valueAdd);
			writer.Write(value.valuePre);
			writer.Write((int)value.atkType);
			writer.Write(value.canAtk);
		}
		public static ATKData ReadATKData(this NetworkReader reader)
		{
			ATKData value = new ATKData();
			value.id = reader.ReadInt();
			value.pre = reader.ReadFloat();
			value.value = reader.ReadFloat();
			value.valueAdd = reader.ReadFloat();
			value.valuePre = reader.ReadFloat();
			value.atkType = (AtkType)reader.ReadInt();
			value.canAtk = reader.ReadBool();
			return value;
		}
	}
}