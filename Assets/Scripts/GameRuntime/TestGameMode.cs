using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode:Base_GameMode
{
    public int JoinPlayerNum = 3; 
    public override void InitPlayer(D_Base_Player Player)
    {
        //血量重置
        Player.runtime_HP = Player.HP_Max;
        Player.runtime_HP_Max = Player.HP_Max;
        //速度与速度上限重置
        Player.runtime_Speed = Player.Speed;
        Player.runtime_Speed_Max = Player.Speed;
        //终极技能初始化
        Player.runtime_ultimate_Skill = Player.ultimate_Skill_Start;
        Player.runtime_ultimate_Skill_Need = Player.ultimate_Skill_Need;
        //位置初始化
        Player.runtime_gird_Position = Player.gird_Position_Start;
        //大小归一
        Player.runtime_characterFigure = 1;
        //改写墨水归一
        Player.runtime_rewrite_ink_NeedRate = Player.rewrite_ink_NeedRate;
        Player.runtime_rewrite_ink_Max = Player.rewrite_ink_Max;
        Player.runtime_rewrite_ink = 0;
        //自己的使魔列表清空
        Player.runtime_myServitors?.Clear();
        //buff清空
        Player.runtime_Buff?.Clear();
        //道具情况
        Player.runtime_Tools?.Clear();
    }
}
