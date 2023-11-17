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

    private List<CharacterData> characters ;
    public List<CharacterData> Characters => characters;

    private Dictionary<string, UIData> uiDataDic;

    private List<PropData> propDatas;
    public List<PropData> PropDatas => propDatas;

    private List<AttackData> attackDatas;
    public List<AttackData> AttackDatas => attackDatas;

    private List<SpriteData> spriteDataSet;
    public List<SpriteData> SpriteDataSet => spriteDataSet;

    public RoomData roomData;

    public PlayerData playerData;
    public string version = "0.9.0728a";
    public uint steamAppID = 2692220;

    public GameServerType gameServerType => (MyNetworkManager.singleton as MyNetworkManager).gameServerType;

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
        spriteDataSet = ResMgr.Instance.Load<SpriteData_SO>("Data_SO/SpriteData_SO").spriteDataSet;
        playerData.account = Random.Range(0, int.MaxValue).ToString();
        LaodLanguage();
        buffDatas = JsonMgr.Instance.LoadData<List<BuffData>>("BuffData/" + "Chinese" + "BuffData");
        characters = ResMgr.Instance.Load<CharacterData_SO>("Data_SO/CharacterData_SO").characterDatas;
        propDatas = ResMgr.Instance.Load<PropData_SO>("Data_SO/PropData_SO").propDatas;
        attackDatas = ResMgr.Instance.Load<AttackData_SO>("Data_SO/AttackData_SO").attackDatas;
    }
    /// <summary>
    /// ���ݳ�ʼ����������Ҫ����Ƕ��������Ϊ�ϲ�����Ϊ�ն�����
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
        BuffBase buffBase = buffPool.GetBuff(id);
        BuffData buffData = GetBuffData(id);
        buffBase.buffData = buffData;
        buffBase.cdMax = buffData.cd;
        buffBase.energy = buffData.energy;
        buffBase.maxEnergy = buffData.maxEnergy;
        return buffBase;
    }

    public BuffData GetBuffData(int id)
    {
        return buffDataDic[id];
    }

    public CharacterData GetCharacter(int id)
    {
        return characters.Find(i => i.character_Code == id);
    }

    public CharacterData RangeCharacter(int id)
    {
        CharacterData characterData = characters[Random.Range(0, characters.Count)];
        while(characterData.character_Code == id)
        {
            characterData = characters[Random.Range(0, characters.Count)];
        }
        return characterData;
    }

    public PropData GetPropData(int id = 0)
    {
        if(id == 0)
        {
            return PropDatas[Random.Range(1, PropDatas.Count)];
        }
        return PropDatas.Find(i => i.id == id);
    }

    public AttackData GetAttackData(int id)
    {
        return AttackDatas.Find(i => i.id == id);
    }
}
