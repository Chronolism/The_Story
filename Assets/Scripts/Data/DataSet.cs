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
/// ÿһ�������ĵ������ݽṹ
/// </summary>
public class D_Enemy
{

}
/// <summary>
/// ��ҵ���Ϣ����
/// </summary>
public class D_Base_Player
{

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