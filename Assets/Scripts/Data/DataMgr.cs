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
        {"Chinese" , "中文"},
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
    /// 数据加载
    /// </summary>
    void DataLosd()
    {
        playerData = JsonMgr.Instance.LoadData<PlayerData>("playerData");
        LaodLanguage();
        buffDatas = JsonMgr.Instance.LoadData<List<BuffData>>("BuffData/" + "Chinese" + "BuffData");
    }
    /// <summary>
    /// 数据初始化（尽量不要出现嵌套数据因为上层数据为空而报错）
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
    /// 保存数据（o：数据，注意保证DataMgr有且名字一样，name：数据名字）
    /// </summary>
    /// <param name="o">数据</param>
    /// <param name="name">数据名字</param>
    public void SaveData(object o, string name)
    {
        JsonMgr.Instance.SaveData(o, name);
    }

    /// <summary>
    /// 加载语言
    /// </summary>
    public void LaodLanguage()
    {
        uiDataDic = new Dictionary<string, UIData>();
    }
    /// <summary>
    /// 获取UI面板的文字数据
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
    /// 设置修改
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
    /// 获取buff
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuffBase GetBuff(int id)
    {
        return buffPool.GetBuff(id);
    }
}
