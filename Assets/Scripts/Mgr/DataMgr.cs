using Mirror.SimpleWeb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : BaseManager<DataMgr>
{
    //敌人数据
    private List<EnemyData> enemyData;
    public List<EnemyData> EnemyData => enemyData;
    //buff数据
    public List<BuffData> buffDatas;
    public Dictionary<int, BuffData> buffDataDic;
    public BuffPool buffPool = new BuffPool();
    //人物数据
    private List<CharacterData> characters ;
    public List<CharacterData> Characters => characters;
    //ui数据
    private Dictionary<string, UIData> uiDataDic;
    //道具数据
    private List<PropData> propDatas;
    public List<PropData> PropDatas => propDatas;
    //攻击数据
    private List<AttackData> attackDatas;
    public List<AttackData> AttackDatas => attackDatas;
    //图片数据集
    private List<SpriteData> spriteDataSet;
    public List<SpriteData> SpriteDataSet => spriteDataSet;
    //特效数据
    public List<EffectData> effectDatas;
    public List<EffectData> EffectDatas => effectDatas;
    public Dictionary<int, EffectData> effectDataDic;

    //瓦片数据集
    private List<TileData> tileDataSet;
    public List<TileData> TileDataSet => tileDataSet;
    private Dictionary<int, TileData> tileDataDic;

    public RoomData roomData;

    public PlayerData playerData;
    public string version = "0.9.0728a";
    public uint steamAppID = 2692220;

    public GameServerType gameServerType => (MyNetworkManager.singleton as MyNetworkManager).gameServerType;

    public Dictionary<string, string> Language = new Dictionary<string, string>()
    {
        {"Chinese" , "中文"},
        { "English","English" }
    };

    public Player activePlayer;
    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    public DataMgr()
    {
        
    }
    /// <summary>
    /// 数据加载
    /// </summary>
    public void DataLoad()
    {
        playerData = ResMgr.Instance.LoadJson<PlayerData>("playerData");
        spriteDataSet = ResMgr.Instance.Load<SpriteData_SO>("Data_SO/SpriteData_SO").spriteDataSet;
        playerData.account = Random.Range(0, int.MaxValue).ToString();
        LaodLanguage();
        buffDatas = ResMgr.Instance.LoadJson<List<BuffData>>("BuffData\\" + "Chinese" + "BuffData");
        characters = ResMgr.Instance.Load<CharacterData_SO>("Data_SO/CharacterData_SO").characterDatas;
        propDatas = ResMgr.Instance.Load<PropData_SO>("Data_SO/PropData_SO").propDatas;
        attackDatas = ResMgr.Instance.Load<AttackData_SO>("Data_SO/AttackData_SO").attackDatas;
        effectDatas = ResMgr.Instance.Load<EffectData_SO>("Data_SO/EffectData_SO").effects;
        tileDataSet = ResMgr.Instance.Load<TileData_SO>("Data_SO/TileData_SO").tileDatas;
        DataInit();
    }
    /// <summary>
    /// 数据初始化（尽量不要出现嵌套数据因为上层数据为空而报错）
    /// </summary>
    void DataInit()
    {
        uiDataDic = new Dictionary<string, UIData>();

        buffDataDic = new Dictionary<int, BuffData>();
        string Path = Application.dataPath + "/ArtRes/UI/CharacterIcon/";
        List<Sprite> buffIcon = spriteDataSet.Find(i => i.name == "buff").sprites;
        foreach (BuffData buff in buffDatas)
        {
            buff.img = buffIcon[buff.spriteIndex];
            
            buffDataDic.Add(buff.id, buff);
        }
        effectDataDic = new Dictionary<int, EffectData>();
        foreach (EffectData effect in effectDatas)
        {
            effectDataDic.Add(effect.id, effect);
        }
        tileDataDic = new Dictionary<int, TileData>();
        foreach(TileData tileData in tileDataSet)
        {
            tileDataDic.Add(tileData.id, tileData);
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
        BuffBase buffBase = buffPool.GetBuff(id);
        BuffData buffData = GetBuffData(id);
        buffBase.buffData = buffData;
        buffBase.cdMax = buffData.cd;
        buffBase.energy = buffData.energy;
        buffBase.maxEnergy = buffData.maxEnergy;
        return buffBase;
    }
    /// <summary>
    /// 获取buff数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuffData GetBuffData(int id)
    {
        return buffDataDic[id];
    }
    /// <summary>
    /// 获取特效数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EffectData GetEffectData(int id)
    {
        return effectDataDic[id];
    }
    /// <summary>
    /// 获取人物数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterData GetCharacter(int id)
    {
        return characters.Find(i => i.character_Code == id);
    }
    /// <summary>
    /// 随机人物排除指定人物
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterData RangeCharacter(int id)
    {
        CharacterData characterData = characters[Random.Range(0, characters.Count)];
        while(characterData.character_Code == id)
        {
            characterData = characters[Random.Range(0, characters.Count)];
        }
        return characterData;
    }
    /// <summary>
    /// 获取道具数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PropData GetPropData(int id = 0)
    {
        if(id == 0)
        {
            return PropDatas[Random.Range(1, PropDatas.Count)];
        }
        return PropDatas.Find(i => i.id == id);
    }
    /// <summary>
    /// 获取攻击数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AttackData GetAttackData(int id)
    {
        return AttackDatas.Find(i => i.id == id);
    }
    /// <summary>
    /// 获取瓦片数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TileData GetTileData(int id)
    {
        return tileDataDic[id];
    }
}
