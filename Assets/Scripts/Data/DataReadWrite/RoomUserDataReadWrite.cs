using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomUserDataReadWrite 
{
    public static void WriteRoomUserData(this NetworkWriter writer, RoomUserData value)
    {
        writer.Write(value.connectId);
        writer.Write(value.characterId);
        writer.Write(value.name);
        writer.Write(value.skills.Count);
        foreach(var i in value.skills)
        {
            writer.Write(i);
        }
    }

    public static RoomUserData ReadRoomUserData(this NetworkReader reader)
    {
        RoomUserData value = new RoomUserData();
        value.connectId = reader.ReadInt();
        value.characterId = reader.ReadInt();
        value.name = reader.ReadString();
        value.skills = new List<BuffDetile>();
        int count = reader.ReadInt();
        for(int i = 0; i < count; i++)
        {
            value.skills.Add(reader.ReadBuffDetile());
        }
        return value;
    }
}
