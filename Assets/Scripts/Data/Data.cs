using Mirror;
using Mirror.Discovery;
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
    public string characterName;
    public RuntimeAnimatorController controller;
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
    public bool ifSure;
    public List<BuffDetile> skills = new List<BuffDetile>();
    public List<int> tags = new List<int>();
    public NetworkConnection con;
    public RoomUserData(int connectId, int characterId, string name, bool ifSure, List<BuffDetile> skills, List<int> tags)
    {
        this.connectId = connectId;
        this.characterId = characterId;
        this.name = name;
        this.ifSure = ifSure;
        this.skills = skills;
        this.tags = tags;
    }

    public RoomUserData(RoomUserData roomUserData)
    {
        connectId = roomUserData.connectId;
        characterId = roomUserData.characterId;
        name = roomUserData.name;
        ifSure = roomUserData.ifSure;
        skills = roomUserData.skills;
        tags = roomUserData.tags;
        con = roomUserData.con;
    }

    public RoomUserData() { }
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
[Serializable]
public class InkData
{
    public float inkAmount;
    public float energyAmount;
    public bool ifTurn;
    public InkData() { }
    public InkData(float inkAmount, float energyAmount, bool ifTurn)
    {
        this.inkAmount = inkAmount;
        this.energyAmount = energyAmount;
        this.ifTurn = ifTurn;
    }
    
}
[Serializable]
public class AttackData
{
    public int id;
    public string name;
    public string description;
    public Sprite img;
    public GameObject gameObject;
}

public class FriendRoom
{
    public string Name;
    public ServerResponse localIP;
    public ulong steamIP;
}
[Serializable]
public class SpriteData
{
    public string name;
    public List<Sprite> sprites;
}