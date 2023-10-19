using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Servitor : MonoBehaviour
{
    public D_Servitor d_servitor;

    public virtual void InitServitor()
    {
        //主人的id(0指完全中立)
        d_servitor.master_runtime_id = 0;
        //生命
        d_servitor.HP_Max = 100;
        d_servitor.runtime_HP_Max = 100;
        d_servitor.runtime_HP = 100;
        //速度
        d_servitor.Speed = 1;
        d_servitor.runtime_Speed = 1;
        d_servitor.runtime_Speed_Max = 1.5f;
        //攻击
        d_servitor.atkDamage = 1;
        //被改写消耗墨水量
        d_servitor.rewrite_ink_Need = 25;
        //被改写给予的能量
        d_servitor.rewrite_given_ultimate_Need = 10;
        //由游戏模式管理赋予显示的使魔式样(-1指完全中立)
        d_servitor.ServitorDisplay = -1;
    }
}
