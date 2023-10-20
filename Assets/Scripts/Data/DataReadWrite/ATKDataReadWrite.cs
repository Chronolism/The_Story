using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ATKDataReadWrite 
{
    // Start is called before the first frame update
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
