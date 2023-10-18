using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (�������ã���ʹ��D_MapDataDetailOriginal)ԭʼ��ͼ��Ϣ �����ݽṹ
/// </summary>
public class D_MapDataOriginal
{
    //ID�����ƣ�����
    //public int ID { get;}
    //public string name { get;}
    //public string description { get; }
    //public string TileAsset { get; set; }
    //public float blood { get; set; }
}
/// <summary>
/// ԭʼ��ͼ������Ϣ �����ݽṹ
/// </summary>
public class D_MapDataDetailOriginal
{
    public int ID;
    public string name;
    public string description;
    public Dictionary<string, Dictionary<Vector3Int, int>> mapCellData;
    
}
/// <summary>
/// ԭʼ��ͼ������Ϣת��Ϊ������unity�������ݵ����ݵļ��ݰ汾
/// </summary>
[System.Serializable]
public class D_MapDataDetailOriginal_Serializable//�����ڴ�ȡ��ʱ��ʹ�õ���ת
{
    public int ID;
    public string name;
    public string description;
    public Dictionary<string, Dictionary<V2, int>> mapCellDataSerializable;
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
    //buff id
    public int buff_Code;
    //buff����
    public string buff_Name;
    //buff����
    public string buff_Description;
    //buff����
    public string buff_Type;
    //buff�ͷŶ���
    public GameObject owner;
    //buff���ö���
    public List<GameObject> buff_Target_GameObjects;
    //buff������ʱ��
    public float buff_Time_Max;
    //buffģ������
    public string[] buff_Module_Type;
    //buffģ��id
    public int[] buff_Module_Code;

    //buff�ͷ�ʱ��Ч����
    [HideInInspector]
    public string buff_FxName_AtRelease;
    //buff����ʱ��Ч����
    [HideInInspector]
    public string buff_FxName_AtDuration;
    //buff����ʱ��Ч����
    [HideInInspector]
    public string buff_FxName_AtEnd;
    //buffͼ��
    [HideInInspector]
    public Sprite icon;

    //buffԤ�Ƽ�����
    public string buff_Prefab_Name;
    //buffԤ�Ƽ�����
    public GameObject buff_Prefab;



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
    public GameObject runtime_Player;
    public int runtime_id = 400;
    //���ʹ�õĽ�ɫ
    public int character_Code;
    //����Ϸģʽ��������ʾ��ʹħʽ��
    public int ownServitorDisplay;
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
    public float runtime_characterFigure;
    public List<string> special_Tags;
    //��д
    public float rewrite_ink_NeedRate;
    public float rewrite_ink_Max;
    public float runtime_rewrite_ink_NeedRate;
    public float runtime_rewrite_ink_Max;
    public float runtime_rewrite_ink;
    public float rewrite_ink_MaxLastTime;
    //ʹħ
    public List<D_Servitor> runtime_myServitors;
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
    public int defaultRuntime_id = 406;
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