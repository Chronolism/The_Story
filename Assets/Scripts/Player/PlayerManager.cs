using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    private D_LocalPlayer _LocalPlayer = new D_LocalPlayer();
    private List<D_OtherPlayer> _OtherPlayers = new List<D_OtherPlayer>();
    /// <summary>
    /// ��ȡ������Ϣ
    /// </summary>
    public D_LocalPlayer LocalPlayer => _LocalPlayer;
    /// <summary>
    /// ��ȡ���������Ϣ
    /// </summary>
    public List<D_OtherPlayer> OtherPlayers => _OtherPlayers;


    /* �����ô���ϰ�߿��ƱȽϺ�
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


    //��������Ϊ���������ڵ���������
    public int character_Code;
    public float HP_Max;
    public float Speed;
    public float atkDamage;
    public float ultimate_Skill_Need;
    public float ultimate_Skill_Start;
    public V2 gird_Position_Start;
    public List<string> special_Tags;
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float rewrite_ink_MaxLastTime;
    public List<int> skill_Index;

    public GameObject runtime_Player;
    public int runtime_id;

    /// <summary>
    /// ����ģʽ��ʼ����ɫ�������
    /// </summary>
    public void InitLocalPlayer()
    {
        //�����ű��Ĵ����������Ӧ��
        _LocalPlayer.character_Code = character_Code;
        _LocalPlayer.HP_Max = HP_Max;
        _LocalPlayer.Speed = Speed;
        _LocalPlayer.atkDamage = atkDamage;
        _LocalPlayer.ultimate_Skill_Need = ultimate_Skill_Need;
        _LocalPlayer.ultimate_Skill_Start = ultimate_Skill_Start;
        _LocalPlayer.gird_Position_Start = gird_Position_Start;
        _LocalPlayer.special_Tags = special_Tags;
        _LocalPlayer.rewrite_ink_NeedRate = rewrite_ink_NeedRate;
        _LocalPlayer.rewrite_ink_Max = rewrite_ink_Max;
        _LocalPlayer.rewrite_ink_MaxLastTime = rewrite_ink_MaxLastTime;
        _LocalPlayer.skill_Index = skill_Index;
        //������Ϸģʽ����ʼ�����ؽ�ɫ
        GameRuntimeManager.Instance.nowaGameMode.InitPlayer(_LocalPlayer);
    }

    public void AddOtherPlayer()
    {
        //GameRuntimeManager.Instance.nowaGameMode.InitPlayer(_OtherPlayer);
    }
    /// <summary>
    /// һ��ͨ��runtime_id��ȡ������ݼ��ķ���
    /// </summary>
    /// <param name="runtime_id">D_Base_Player��һ��</param>
    /// <returns></returns>
    public D_Base_Player GetPlayerDataWithRuntime_Id(int runtime_id)
    {
        if (runtime_id == _LocalPlayer.runtime_id)
            return _LocalPlayer;
        else
        {
            foreach (var item in _OtherPlayers)
            {
                if(runtime_id == item.runtime_id)
                    return item;
            }
            return null;
        }
    }
}
public static class exploreFunc
{
    public static void runtimeStart(this D_Base_Player d_Base_Player)
    {
        
    }
}

