using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    private D_LocalPlayer _LocalPlayer = new D_LocalPlayer();
    private List<D_OtherPlayer> _OtherPlayers = new List<D_OtherPlayer>();
    /// <summary>
    /// 获取本机信息
    /// </summary>
    public D_LocalPlayer LocalPlayer => _LocalPlayer;
    /// <summary>
    /// 获取其他玩家信息
    /// </summary>
    public List<D_OtherPlayer> OtherPlayers => _OtherPlayers;


    /* 或许用代码习惯控制比较好
    public float My_HP_Max { get { return _LocalPlayer.runtime_HP_Max; }set { _LocalPlayer.runtime_HP_Max = value; } }
    public float My_HP { get { return _LocalPlayer.runtime_HP; } set { _LocalPlayer.runtime_HP = value; } }
    public float My_Speed { get { return _LocalPlayer.runtime_Speed; } set { _LocalPlayer.runtime_Speed = value; } }
    public float My_Speed_Max { get { return _LocalPlayer.runtime_Speed_Max; } set { _LocalPlayer.runtime_Speed_Max = value; } }
    public float runtime_ultimate_Skill;
    public float runtime_ultimate_Skill_Need;
    public V2 runtime_gird_Position;
    public float runtime_characterFigure;
    public float runtime_rewrite_ink_NeedRate;
    public float runtime_rewrite_ink_Max;
    public float runtime_rewrite_ink;
    public List<D_Servitor> runtime_Servitors;
    public List<D_Buff> runtime_Buff;
    public List<int> runtime_Tools;
    public List<string> special_Tags;
    */
    //玩家使用的角色
    public int character_Code;
    //生命
    public float HP_Max;
    //速度
    public float Speed;
    //攻击
    public float atkDamage;
    //终极技能
    public float ultimate_Skill_Need;
    public float ultimate_Skill_Start;
    //位置
    public V2 gird_Position_Start;
    //状态
    public List<string> special_Tags;
    //改写
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float rewrite_ink_MaxLastTime;
    //使魔
    public List<int> skill_Index;
}

public static class exploreFunc
{
    public static void runtimeStart(this D_Base_Player d_Base_Player)
    {
        
    }
}

