using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomUserDataReadWrite 
{
    public static void WriteATKData(this NetworkWriter writer, RoomUserData value)
    {
        writer.Write(value.characterId);
        writer.Write(value.name);
    }

    public static RoomUserData ReadATKData(this NetworkReader reader)
    {
        RoomUserData value = new RoomUserData();
        value.characterId = reader.ReadInt();
        value.name = reader.ReadString();
        return value;
    }
}
