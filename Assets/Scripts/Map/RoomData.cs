using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : NetworkBehaviour
{
    [SyncVar(hook = "UpDataDetile")]
    public string mapName = "400";
    [SyncVar]
    public string HostUser = "";
    public readonly SyncIDictionary<string, RoomUserData> roomUser = new SyncIDictionary<string, RoomUserData>(new Dictionary<string, RoomUserData>());
    public List<Observer<RoomData>> observers = new List<Observer<RoomData>>();

    [SyncVar]
    public bool ifGame;
    [SyncVar]
    public float startGameTime;
    [SyncVar]
    public bool ifPause = true;

    public RoomLogicBase roomLogic;

    public void AddRoomUser(string name , NetworkConnection con)
    {
        if (HostUser == "") HostUser = name;
        RoomUserData roomUserData = new RoomUserData() { name = name ,connectId = con.connectionId , con = con};
        if (roomUser.ContainsKey(name)) return;
        roomUser.Add(name, roomUserData);
    }

    public void Awake()
    {
        DataMgr.Instance.roomData = this;
    }

    public void UpDataDetile(string oldvalue , string  value)
    {
        foreach(var a in observers)
        {
            a.ToUpdate(this);
        }
    }
    /// <summary>
    /// 呼叫服务器开始游戏
    /// </summary>
    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        GameMgr.Instance.StartGame();
        roomLogic = new NormalRoom(this);
        roomLogic.ToStartGame();
        StartCoroutine(StartGameCountdown());
    }

    IEnumerator StartGameCountdown()
    {
        startGameTime = 2;
        while (startGameTime > 0)
        {
            startGameTime -= Time.deltaTime;
            yield return null;
        }
        BeginGame();
    }
    /// <summary>
    /// 修改用户属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="roomUserData"></param>
    [Command(requiresAuthority = false)]
    public void ChangeRoomUserData(string name ,RoomUserData roomUserData)
    {
        roomUser[name] = roomUserData;
    }

    /// <summary>
    /// 服务器加载地图
    /// </summary>
    /// <param name="name"></param>
    [Server]
    public void LoadMap(string name)
    {
        GameMgr.Instance.LoadMap(name);
        LoadMapRpc(name);
    }
    /// <summary>
    /// 客户端加载地图
    /// </summary>
    /// <param name="name"></param>
    [ClientRpc]
    public void LoadMapRpc(string name)
    {
        if (isServer) return;
        GameMgr.Instance.LoadMap(name);
    }
    /// <summary>
    /// 服务器开始游戏
    /// </summary>
    [Server]
    public void BeginGame()
    {
        UIManager.Instance.ClearAllPanel();
        roomLogic.StartGame();
        EventMgr.CallStartGame();
        ifPause = false;
        BeginGaneRPC();
    }
    /// <summary>
    /// 客户端开始游戏
    /// </summary>
    [ClientRpc]
    public void BeginGaneRPC()
    {
        if (isServer) return;
        UIManager.Instance.ClearAllPanel();
        //roomLogic.StartGame();
        EventMgr.CallStartGame();
    }

}
