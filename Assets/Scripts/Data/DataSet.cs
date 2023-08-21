using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ԭʼ��ͼ��Ϣ �����ݽṹ
/// </summary>
public class D_MapDataOriginal
{
    //ID�����ƣ�����
    public int ID { get;}
    public string name { get;}
    public string description { get; }
    //public string TileAsset { get; set; }
    //public float blood { get; set; }
}
/// <summary>
/// ԭʼ��ͼ������Ϣ �����ݽṹ
/// </summary>
public class D_MapDataDetailOriginal
{

}
/// <summary>
/// ԭʼ��ͼ������Ϣת��Ϊjson����
/// </summary>
public class D_MapDataDetailOriginal_ToJson
{

}
/// <summary>
/// ÿһ��������UI�ؼ����ݽṹ
/// </summary>
public class D_UI
{
    //�ؼ����� ����
    public Dictionary<string, string> value;
}
/// <summary>
/// ÿһ��������buff���ݽṹ
/// </summary>
public class D_Buff
{

}
/// <summary>
/// ÿһ��������ʹħ���ݽṹ
/// </summary>
public class D_Servitor
{

}
/// <summary>
/// ��ҵ���Ϣ����
/// </summary>
public class D_Base_Player
{
    //���ʹ�õĽ�ɫ
    public int character_Code;
    //����
    public float HP_Max;
    public float runtime_HP_Max;
    public float runtime_HP;
    //�ٶ�
    public float Speed;
    public float runtime_Speed;
    public float runtime_Speed_Max;
    //����
    public float atkDamage;
    //�ռ�����
    public float ultimate_Skill_Need;
    public float ultimate_Skill_Start;
    public float runtime_ultimate_Skill;
    public float runtime_ultimate_Skill_Need;
    //λ��
    public V2 gird_Position_Start;
    public V2 runtime_gird_Position;
    //״̬
    public float characterFigure;
    public List<string> special_Tags;
    //��д
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float runtime_rewrite_ink_NeedRate;
    public float runtime_rewrite_ink_Max;
    public float runtime_rewrite_ink;
    public float rewrite_ink_MaxLastTime;
    //ʹħ
    public List<D_Servitor> runtime_Servitors;
    //����
    public List<int> skill_Index;
    public List<D_Buff> runtime_Buff;
    //����
    public List<int> runtime_Tools;
}
/// <summary>
/// ���ز��ݵ������Ϣ
/// </summary>
public class D_LocalPlayer: D_Base_Player
{

}
/// <summary>
/// �Ǳ��ز��ݵ������Ϣ
/// </summary>
public class D_OtherPlayer : D_Base_Player
{

}
/// <summary>
/// һ����Ϸ����ʱ�ĸ�������
/// </summary>
public class D_GameRuntime
{

}
/// <summary>
/// ��ҵ��˻����ݽṹ
/// </summary>
public class D_PlayerAccount
{

}
/// <summary>
/// ��ҵı��س־û���������Ϣ
/// </summary>
public class D_PlayerLocal
{

}