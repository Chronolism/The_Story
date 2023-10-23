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
    public List<string> special_Tags;
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float rewrite_ink_MaxLastTime;
    public List<int> skill_Index;

    // ������ͼ��
    public V2 gird_Position_Start => new V2(GameRuntimeManager.Instance.nowaGameMode.GetPlayerStartPos().x, GameRuntimeManager.Instance.nowaGameMode.GetPlayerStartPos().y);

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
        //��ʼ��λ��
        _LocalPlayer.gird_Position_Start = gird_Position_Start;
        //������Ϸģʽ����ʼ�����ؽ�ɫ
        GameRuntimeManager.Instance.nowaGameMode.InitPlayer(_LocalPlayer);
    }

    public void AddOtherPlayer(uint netId , D_Base_Player d_Base_Player)
    {
        D_OtherPlayer d_OtherPlayer = new D_OtherPlayer();
        if (GetPlayerDataWithRuntime_Id(d_Base_Player.runtime_id) != null)
        {
            if(GetPlayerDataWithRuntime_Id(d_Base_Player.runtime_id) is D_OtherPlayer)
            {
                _OtherPlayers.Remove(GetPlayerDataWithRuntime_Id(d_Base_Player.runtime_id) as D_OtherPlayer);
            }
        }
        d_OtherPlayer.character_Code = d_Base_Player.character_Code;
        d_OtherPlayer.HP_Max = d_Base_Player.HP_Max;
        d_OtherPlayer.Speed = d_Base_Player.Speed;
        d_OtherPlayer.atkDamage = d_Base_Player.atkDamage;
        d_OtherPlayer.ultimate_Skill_Need = d_Base_Player.ultimate_Skill_Need;
        d_OtherPlayer.ultimate_Skill_Start = d_Base_Player.ultimate_Skill_Start;
        d_OtherPlayer.gird_Position_Start = d_Base_Player.gird_Position_Start;
        d_OtherPlayer.special_Tags = d_Base_Player.special_Tags;
        d_OtherPlayer.rewrite_ink_NeedRate = d_Base_Player.rewrite_ink_NeedRate;
        d_OtherPlayer.rewrite_ink_Max = d_Base_Player.rewrite_ink_Max;
        d_OtherPlayer.rewrite_ink_MaxLastTime = d_Base_Player.rewrite_ink_MaxLastTime;
        d_OtherPlayer.skill_Index = d_Base_Player.skill_Index;
        d_OtherPlayer.runtime_id = netId;
        GameRuntimeManager.Instance.nowaGameMode.InitPlayer(d_OtherPlayer);
        _OtherPlayers.Add(d_OtherPlayer);
    }
    public void AddOtherPlayerForOfflineMode(bool ifUseSameDataWithLocal = true)
    {
        if (ifUseSameDataWithLocal)
        {
            D_OtherPlayer d_OtherPlayer = new D_OtherPlayer();
            d_OtherPlayer.character_Code = character_Code;
            d_OtherPlayer.HP_Max = HP_Max;
            d_OtherPlayer.Speed = Speed;
            d_OtherPlayer.atkDamage = atkDamage;
            d_OtherPlayer.ultimate_Skill_Need = ultimate_Skill_Need;
            d_OtherPlayer.ultimate_Skill_Start = ultimate_Skill_Start;
            d_OtherPlayer.gird_Position_Start = gird_Position_Start;
            d_OtherPlayer.special_Tags = special_Tags;
            d_OtherPlayer.rewrite_ink_NeedRate = rewrite_ink_NeedRate;
            d_OtherPlayer.rewrite_ink_Max = rewrite_ink_Max;
            d_OtherPlayer.rewrite_ink_MaxLastTime = rewrite_ink_MaxLastTime;
            d_OtherPlayer.skill_Index = skill_Index;
            GameRuntimeManager.Instance.nowaGameMode.InitPlayer(d_OtherPlayer);

            //�������Ĭ�ϵ�runtime_idΪ400���������Ĭ�ϵ�runtime_idΪ406
            d_OtherPlayer.runtime_id = d_OtherPlayer.defaultRuntime_id;//(406)

            _OtherPlayers.Add(d_OtherPlayer);
        }
    }
    
    /// <summary>
    /// һ��ͨ��runtime_id��ȡ������ݼ��ķ���
    /// </summary>
    /// <param name="runtime_id">D_Base_Player��һ��</param>
    /// <returns></returns>
    public D_Base_Player GetPlayerDataWithRuntime_Id(uint runtime_id)
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

public enum E_PlayerControlMode
{
    Pacman,
    Free4,
    Free8,
}