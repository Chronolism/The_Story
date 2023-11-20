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
    public List<Prop> nullToolProp = new List<Prop>();
    public List<Prop> toolProps = new List<Prop>();
    public List<Prop> nullFeatherProp = new List<Prop>();
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
        roomData.StartGame();
    }

    public override void StartGame()
    {

        roomData.StartCoroutine(StartGameCountdown());
    }
    /// <summary>
    /// 先用协程模拟各用户加载延迟
    /// </summary>
    /// <returns></returns>
    IEnumerator StartGameCountdown()
    {
        float startGameTime = 4;
        EntityFactory.Instance.ClearNetworkEntity();
        nullToolProp.Clear();
        toolProps.Clear();
        nullFeatherProp.Clear();
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
        Debug.Log("Start");
        UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
        {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                UIManager.Instance.ClearAllPanel();
                UIManager.Instance.ShowPanel<GamePanel>();
            });
        }, true);
    }



    public override void LoadMap()
    {
        //遍历所有道具点生成空道具
        foreach (Vector3 v3 in cellsForToolsBorn)
        {
            Prop prop = EntityFactory.Instance.CreatProp(v3);
            nullToolProp.Add(prop);
            //添加道具观察
            prop.observers.Add(this);
        }
        foreach (Vector3 v3 in cellsForFeatherPenBorn)
        {
            Prop prop = EntityFactory.Instance.CreatProp(v3);
            nullFeatherProp.Add(prop);
            //添加道具观察
            prop.observers.Add(this);
        }
        //显示一个羽毛id = 1和道具 id = 2;
        RollNullFeatherProp()?.ShowProp(DataMgr.Instance.GetPropData(1));
        RollNullToolProp()?.ShowProp(DataMgr.Instance.GetPropData(2));
        //在随机使魔生成点生成一个使魔
        EntityFactory.Instance.CreatServitor(cellsForServitorBorn[Random.Range(0, cellsForServitorBorn.Count)], roomData.ifPause);
    }

    public override void LoadPlayer()
    {
        //遍历玩家
        foreach (var user in roomData.roomUser)
        {
            //在随机玩家生成点生成玩家
            Player player = EntityFactory.Instance.CreatPlayer(user, cellsForPlayerBorn[Random.Range(0, cellsForPlayerBorn.Count)]);
            player.observers.Add(this);
            playerList.Add(player);
        }
    }

    public override void BeginGame()
    {
        foreach (var player in DataMgr.Instance.players)
        {
            if (player.Value.userName == DataMgr.Instance.playerData.account)
            {
                DataMgr.Instance.activePlayer = player.Value;
                break;
            }
        }
        EventMgr.CallStartGame();
        EventMgr.CallContinueGame();
    }


    public override void BeginGameClient()
    {
        if (!roomData.isServer)
        {
            foreach (var player in DataMgr.Instance.players)
            {
                if (player.Value.userName == DataMgr.Instance.playerData.account)
                {
                    DataMgr.Instance.activePlayer = player.Value;
                    break;
                }
            }
            EventMgr.CallStartGame();
            EventMgr.CallContinueGame();
        }
    }

    public override void FinishGame()
    {
        EventMgr.CallPauseGame();
        
    }

    public override void FinishGameClient()
    {
        UIManager.Instance.ShowPanel<FinishGamePanel>(null, true);
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
                    RollNullToolProp()?.ShowProp(DataMgr.Instance.GetPropData());
                }
                if (featherProps.Count < 2)
                {
                    RollNullFeatherProp()?.ShowProp(DataMgr.Instance.GetPropData(1));
                }
            }
            if (time < 60 && servitor > 15)
            {
                servitor = 0;
                EntityFactory.Instance.CreatServitor(cellsForServitorBorn[Random.Range(0, cellsForServitorBorn.Count)], roomData.ifPause);
            }
            else if (time >= 60 && servitor > 10) 
            {
                servitor = 0;
                EntityFactory.Instance.CreatServitor(cellsForServitorBorn[Random.Range(0, cellsForServitorBorn.Count)], roomData.ifPause);
            }
        }

    }
    public override void EndGame()
    {
        EntityFactory.Instance.ClearNetworkEntity();
    }

    public override void EndGameClient()
    {
        UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
        {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                UIManager.Instance.ClearAllPanel();
                UIManager.Instance.ShowPanel<RoomPanel>();
            });
        }, true);

    }

    /// <summary>
    /// 观察到道具消失，添加到空道具列表
    /// </summary>
    /// <param name="value"></param>
    public void ToUpdate(Prop value)
    {
        if(value.propid== 1)
        {
            nullFeatherProp.Add(value);
        }
        else
        {
            nullToolProp.Add(value);
        }
        
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
    /// 从空道具列表中随机拿一个
    /// </summary>
    /// <returns></returns>
    Prop RollNullToolProp()
    {
        if (nullToolProp.Count > 0)
        {
            int i = Random.Range(0, nullToolProp.Count);
            Prop prop = nullToolProp[i];
            nullToolProp.RemoveAt(i);
            return prop;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 从空道具列表中随机拿一个
    /// </summary>
    /// <returns></returns>
    Prop RollNullFeatherProp()
    {
        if (nullFeatherProp.Count > 0)
        {
            int i = Random.Range(0, nullFeatherProp.Count);
            Prop prop = nullFeatherProp[i];
            nullFeatherProp.RemoveAt(i);
            return prop;
        }
        else
        {
            return null;
        }
    }
}
