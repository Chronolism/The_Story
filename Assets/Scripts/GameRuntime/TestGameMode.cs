using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode:Base_GameMode
{
    public int JoinPlayerNum = 3; 
    public override void InitPlayer(D_Base_Player Player)
    {
        //Ѫ������
        Player.runtime_HP = Player.HP_Max;
        Player.runtime_HP_Max = Player.HP_Max;
        //�ٶ����ٶ���������
        Player.runtime_Speed = Player.Speed;
        Player.runtime_Speed_Max = Player.Speed;
        //�ռ����ܳ�ʼ��
        Player.runtime_ultimate_Skill = Player.ultimate_Skill_Start;
        Player.runtime_ultimate_Skill_Need = Player.ultimate_Skill_Need;
        //λ�ó�ʼ��
        Player.runtime_gird_Position = Player.gird_Position_Start;
        //��С��һ
        Player.runtime_characterFigure = 1;
        //��дīˮ��һ
        Player.runtime_rewrite_ink_NeedRate = Player.rewrite_ink_NeedRate;
        Player.runtime_rewrite_ink_Max = Player.rewrite_ink_Max;
        Player.runtime_rewrite_ink = 0;
        //�Լ���ʹħ�б����
        Player.runtime_myServitors?.Clear();
        //buff���
        Player.runtime_Buff?.Clear();
        //�������
        Player.runtime_Tools?.Clear();
    }
}
