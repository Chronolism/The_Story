using Mirror.SimpleWeb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : BaseManager<DataMgr>
{
    private List<EnemyData> enemyData;
    public List<EnemyData> EnemyData => enemyData;

    public List<BuffData> buffDatas;
    public Dictionary<int, BuffData> buffDataDic;
    public BuffPool buffPool = new BuffPool();

    private Dictionary<string, UIData> uiDataDic;

    public PlayerData playerData;
    public string version = "0.9.0728a";

    public Dictionary<string, string> Language = new Dictionary<string, string>()
    {
        {"Chinese" , "����"},
        { "English","English" }
    };

    public Player activePlayer;
    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    public DataMgr()
    {
        DataLosd();
        DataInit();
    }
    /// <summary>
    /// ���ݼ���
    /// </summary>
    void DataLosd()
    {
        playerData = JsonMgr.Instance.LoadData<PlayerData>("playerData");
        LaodLanguage();
        buffDatas = JsonMgr.Instance.LoadData<List<BuffData>>("BuffData/" + "Chinese" + "BuffData");
    }
    /// <summary>
    /// ���ݳ�ʼ����������Ҫ����Ƕ��������Ϊ�ϲ�����Ϊ�ն�����
    /// </summary>
    void DataInit()
    {
        uiDataDic = new Dictionary<string, UIData>();

        buffDataDic = new Dictionary<int, BuffData>();
        foreach (BuffData buff in buffDatas)
        {
            buffDataDic.Add(buff.id, buff);
        }

    }
    /// <summary>
    /// �������ݣ�o�����ݣ�ע�Ᵽ֤DataMgr��������һ����name���������֣�
    /// </summary>
    /// <param name="o">����</param>
    /// <param name="name">��������</param>
    public void SaveData(object o, string name)
    {
        JsonMgr.Instance.SaveData(o, name);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void LaodLanguage()
    {
        uiDataDic = new Dictionary<string, UIData>();
    }
    /// <summary>
    /// ��ȡUI������������
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public UIData GetUIStr(string panelName)
    {
        if (uiDataDic.ContainsKey(panelName))
        {
            return uiDataDic[panelName];
        }
        else
        {
            UIData uidata = JsonMgr.Instance.LoadData<UIData>("UIData/" + playerData.language + "/" + panelName);
            uiDataDic.Add(panelName, uidata);
            return uidata;
        }
    }
    /// <summary>
    /// �����޸�
    /// </summary>
    /// <param name="settingData"></param>
    public void ChangeSetting(SettingData settingData)
    {
        if (settingData.Language != playerData.language)
        {
            playerData.language = settingData.Language;
            LaodLanguage();
        }
    }
    /// <summary>
    /// ��ȡbuff
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuffBase GetBuff(int id)
    {
        return buffPool.GetBuff(id);
    }
}
