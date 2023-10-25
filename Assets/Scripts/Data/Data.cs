using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int id;
    public string mapName;
    public int mod;
}

[Serializable]
public class CharacterData
{
    public int character_Code;
    public float HP_Max;
    public float Speed;
    public float atkDamage;
    public float ultimate_Skill_Need;
    public float ultimate_Skill_Start;
    public List<string> special_Tags;
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float rewrite_ink_MaxLastTime;
    public List<BuffDetile> skill_Index;
}
[Serializable]
public class RoomUserData
{
    public int connectId;
    public int characterId;
    public string name;
    public List<BuffDetile> skills = new List<BuffDetile>();
    public NetworkConnection con;
}
[Serializable]
public class PropData
{
    public int id;
    public string name;
    public Sprite icon;
    public PropType propType;
    public string description;
    public List<BuffDetile> value;
}

public enum PropType
{
    UseTool,
    TrigerTool
}
[Serializable]
public struct BuffDetile
{
    [BuffDetile]
    public int buffId;
    public int buffValue;
}