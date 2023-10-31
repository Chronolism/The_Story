using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoom : RoomLogicBase, Observer<Prop>,Observer<Player>
{
    public float time = 0;
    float timer = 0;
    float servitor;
    float toolTime;
    public List<Prop> nullProp = new List<Prop>();
    public List<Prop> toolProps = new List<Prop>();
    public List<Prop> featherProps = new List<Prop>();

    public List<Player> playerList = new List<Player>();

    public NormalRoom(RoomData roomData):base(roomData)
    {


    }

    public override void OpenGame()
    {
        roomData.roomTags.Clear();
        roomData.roomTags.Add(1);
        foreach (var user in roomData.roomUser)
        {
            user.tags = new List<int>() { 0, 0 };
        }
        StartGame();
    }

    public override void StartGame()
    {

        roomData.StartCoroutine(StartGameCountdown());
    }
    /// <summary>
    /// ����Э��ģ����û������ӳ�
    /// </summary>
    /// <returns></returns>
    IEnumerator StartGameCountdown()
    {
        float startGameTime = 2;
        EntityFactory.Instance.ClearNetworkEntity();
        nullProp.Clear();
        toolProps.Clear();
        featherProps.Clear();
        playerList.Clear();
        time = 0;
        timer = 0;
        servitor = 0;
        toolTime = 0;
        LoadMapData(roomData.mapName);
        LoadMap();
        LoadPlayer();
        while (startGameTime > 0)
        {
            startGameTime -= Time.deltaTime;
            yield return null;
        }
        roomData.BeginGame();
    }

    public override void StartGameClient()
    {
        UIManager.Instance.ClearAllPanel();
    }



    public override void LoadMap()
    {
        //�������е��ߵ����ɿյ���
        foreach(Vector3Int v3 in cellsForToolsBorn)
        {
            Prop prop = EntityFactory.Instance.CreatProp(new Vector3(v3.x + 0.5f, v3.y + 0.5f, 0));
            nullProp.Add(prop);
            //��ӵ��߹۲�
            prop.observers.Add(this);

        }
        //��ʾһ����ëid = 1�͵��� id = 2;
        RollNullProp().ShowProp(DataMgr.Instance.GetPropData(1));
        RollNullProp().ShowProp(DataMgr.Instance.GetPropData(2));
        //�����ʹħ���ɵ�����һ��ʹħ
        //EntityFactory.Instance.CreatServitor(cellsForServitorBorn[Random.Range(0, cellsForServitorBorn.Count)], roomData.ifPause);
    }

    public override void LoadPlayer()
    {
        //�������
        foreach (var user in roomData.roomUser)
        {
            //�����������ɵ��������
            Player player = EntityFactory.Instance.CreatPlayer(user, cellsForPlayerBorn[Random.Range(0, cellsForPlayerBorn.Count)]);
            player.observers.Add(this);
            playerList.Add(player);
        }
    }

    public override void BeginGame()
    {
        
    }


    public override void BeginGameClient()
    {
        UIManager.Instance.ShowPanel<GamePanel>();
        foreach (var player in DataMgr.Instance.players)
        {
            if (player.Value.userName == DataMgr.Instance.playerData.account)
            {
                DataMgr.Instance.activePlayer = player.Value;
                return;
            }
        }
    }

    public override void FinishGame()
    {
        EventMgr.CallPauseGame();
        
    }

    public override void FinishGameClient()
    {
        UIManager.Instance.ShowPanel<FinishGamePanel>();
        if (!roomData.isServer) { EventMgr.CallPauseGame(); }
    }

    public override void Updata()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            time++;
            toolTime++;
            servitor++;
            if (toolTime > 5)
            {
                toolTime = 0;
                if (toolProps.Count < 3)
                {
                    RollNullProp()?.ShowProp(DataMgr.Instance.GetPropData(2));
                }
                if (featherProps.Count < 2)
                {
                    RollNullProp()?.ShowProp(DataMgr.Instance.GetPropData(1));
                }
            }
            if (time < 60 && servitor > 15)
            {
                servitor = 0;
                //EntityFactory.Instance.CreatServitor(cellsForServitorBorn[Random.Range(0, cellsForServitorBorn.Count)], roomData.ifPause);
            }
            else if (time >= 60 && servitor > 10) 
            {
                servitor = 0;
                //EntityFactory.Instance.CreatServitor(cellsForServitorBorn[Random.Range(0, cellsForServitorBorn.Count)], roomData.ifPause);
            }
        }

    }
    public override void EndGame()
    {
        EntityFactory.Instance.ClearNetworkEntity();
    }

    public override void EndGameClient()
    {
        UIManager.Instance.ClearAllPanel();
        UIManager.Instance.ShowPanel<RoomPanel>();
    }

    /// <summary>
    /// �۲쵽������ʧ����ӵ��յ����б�
    /// </summary>
    /// <param name="value"></param>
    public void ToUpdate(Prop value)
    {
        nullProp.Add(value);
    }

    public void ToUpdate(Player value)
    {
        playerList.Remove(value);
        if(playerList.Count == 0)
        {
            return;
        }
        if(playerList.Count == 1)
        {
            for (int i = 0; i < roomData.roomUser.Count; i++)
            {
                if (roomData.roomUser[i].name == playerList[0].userName)
                {
                    RoomUserData roomUserData = new RoomUserData(roomData.roomUser[i]);
                    roomUserData.tags[0]++;
                    roomData.roomUser[i] = roomUserData;
                }
            }
            
            foreach (var user in roomData.roomUser)
            {
                if (user.tags[0] == 3)
                {
                    roomData.EndhGame();
                    return;
                }
            }
            roomData.FinishGame();
        }
    }

    /// <summary>
    /// �ӿյ����б��������һ��
    /// </summary>
    /// <returns></returns>
    Prop RollNullProp()
    {
        if (nullProp.Count > 0)
        {
            int i = Random.Range(0, nullProp.Count);
            Prop prop = nullProp[i];
            nullProp.RemoveAt(i);
            return prop;
        }
        else
        {
            return null;
        }
    }


}
