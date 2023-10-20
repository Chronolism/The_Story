using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<int> skill_Index;
}

public class RoomUserData
{
    public int connectId;
    public int characterId;
    public string name;
    public NetworkConnection con;
}