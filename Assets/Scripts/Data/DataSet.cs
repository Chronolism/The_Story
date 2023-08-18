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
/// 每一个基础的敌人数据结构
/// </summary>
public class D_Enemy
{

}
/// <summary>
/// 玩家的信息父类
/// </summary>
public class D_Base_Player
{

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