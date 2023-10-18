using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (暂已弃用，请使用D_MapDataDetailOriginal)原始地图信息 的数据结构
/// </summary>
public class D_MapDataOriginal
{
    //ID，名称，描述
    //public int ID { get;}
    //public string name { get;}
    //public string description { get; }
    //public string TileAsset { get; set; }
    //public float blood { get; set; }
}
/// <summary>
/// 原始地图网格信息 的数据结构
/// </summary>
public class D_MapDataDetailOriginal
{
    public int ID;
    public string name;
    public string description;
    public Dictionary<string, Dictionary<Vector3Int, int>> mapCellData;
    
}
/// <summary>
/// 原始地图网格信息转化为不包含unity内置内容的数据的兼容版本
/// </summary>
[System.Serializable]
public class D_MapDataDetailOriginal_Serializable//仅仅在存取的时候使用的中转
{
    public int ID;
    public string name;
    public string description;
    public Dictionary<string, Dictionary<V2, int>> mapCellDataSerializable;
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
    //buff id
    public int buff_Code;
    //buff名称
    public string buff_Name;
    //buff描述
    public string buff_Description;
    //buff类型
    public string buff_Type;
    //buff释放对象
    public GameObject owner;
    //buff作用对象
    public List<GameObject> buff_Target_GameObjects;
    //buff最大持续时间
    public float buff_Time_Max;
    //buff模块类型
    public string[] buff_Module_Type;
    //buff模块id
    public int[] buff_Module_Code;

    //buff释放时特效名称
    [HideInInspector]
    public string buff_FxName_AtRelease;
    //buff持续时特效名称
    [HideInInspector]
    public string buff_FxName_AtDuration;
    //buff结束时特效名称
    [HideInInspector]
    public string buff_FxName_AtEnd;
    //buff图标
    [HideInInspector]
    public Sprite icon;

    //buff预制件名称
    public string buff_Prefab_Name;
    //buff预制件对象
    public GameObject buff_Prefab;



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
    public GameObject runtime_Player;
    public int runtime_id = 400;
    //玩家使用的角色
    public int character_Code;
    //由游戏模式管理赋予显示的使魔式样
    public int ownServitorDisplay;
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
    public float runtime_characterFigure;
    public List<string> special_Tags;
    //改写
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float runtime_rewrite_ink_NeedRate;
    public float runtime_rewrite_ink_Max;
    public float runtime_rewrite_ink;
    public float rewrite_ink_MaxLastTime;
    //使魔
    public List<D_Servitor> runtime_myServitors;
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
    public int defaultRuntime_id = 406;
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