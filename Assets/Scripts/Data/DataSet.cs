using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 原始地图信息 的数据结构
/// </summary>
public class D_MapDataOriginal
{
    //ID，名称，描述
    public int ID { get;}
    public string name { get;}
    public string description { get; }
    //public string TileAsset { get; set; }
    //public float blood { get; set; }
}
/// <summary>
/// 原始地图网格信息 的数据结构
/// </summary>
public class D_MapDataDetailOriginal
{

}
/// <summary>
/// 原始地图网格信息转化为json兼容
/// </summary>
public class D_MapDataDetailOriginal_ToJson
{

}
/// <summary>
/// 每一个基础的UI控件数据结构
/// </summary>
public class D_UI
{
    //控件名称 内容
    public Dictionary<string, string> value;
}
/// <summary>
/// 每一个基础的buff数据结构
/// </summary>
public class D_Buff
{

}
/// <summary>
/// 每一个基础的使魔数据结构
/// </summary>
public class D_Servitor
{

}
/// <summary>
/// 玩家的信息父类
/// </summary>
public class D_Base_Player
{
    //玩家使用的角色
    public int character_Code;
    //生命
    public float HP_Max;
    public float runtime_HP_Max;
    public float runtime_HP;
    //速度
    public float Speed;
    public float runtime_Speed;
    public float runtime_Speed_Max;
    //攻击
    public float atkDamage;
    //终极技能
    public float ultimate_Skill_Need;
    public float ultimate_Skill_Start;
    public float runtime_ultimate_Skill;
    public float runtime_ultimate_Skill_Need;
    //位置
    public V2 gird_Position_Start;
    public V2 runtime_gird_Position;
    //状态
    public float characterFigure;
    public List<string> special_Tags;
    //改写
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float runtime_rewrite_ink_NeedRate;
    public float runtime_rewrite_ink_Max;
    public float runtime_rewrite_ink;
    public float rewrite_ink_MaxLastTime;
    //使魔
    public List<D_Servitor> runtime_Servitors;
    //技能
    public List<int> skill_Index;
    public List<D_Buff> runtime_Buff;
    //道具
    public List<int> runtime_Tools;
}
/// <summary>
/// 本地操纵的玩家信息
/// </summary>
public class D_LocalPlayer: D_Base_Player
{

}
/// <summary>
/// 非本地操纵的玩家信息
/// </summary>
public class D_OtherPlayer : D_Base_Player
{

}
/// <summary>
/// 一局游戏进行时的各项数据
/// </summary>
public class D_GameRuntime
{

}
/// <summary>
/// 玩家的账户数据结构
/// </summary>
public class D_PlayerAccount
{

}
/// <summary>
/// 玩家的本地持久化的数据信息
/// </summary>
public class D_PlayerLocal
{

}