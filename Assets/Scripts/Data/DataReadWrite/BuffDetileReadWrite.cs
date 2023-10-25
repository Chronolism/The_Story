using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffDetileReadWrite
{
    public static void WriteBuffDetile(this NetworkWriter writer, BuffDetile value)
    {
        writer.Write(value.buffId);
        writer.Write(value.buffValue);
    }

    public static BuffDetile ReadBuffDetile(this NetworkReader reader)
    {
        BuffDetile value = new BuffDetile();
        value.buffId = reader.ReadInt();
        value.buffValue = reader.ReadInt();
        return value;
    }
}
