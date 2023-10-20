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
    // Start is called before the first frame update

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
    /// 服务器加载全部角色给与操作权限
    /// </summary>
    [Server]
    public void AllLoadCharacter()
    {
        foreach(var user in roomUser)
        {
            EntityFactory.Instance.CreatPlayer(user.Value , new Vector3(0.5f,0.5f,0));
        }
    }
    /// <summary>
    /// 服务器开始游戏
    /// </summary>
    [Server]
    public void BeginGane()
    {
        UIManager.Instance.ClearAllPanel();
        EventMgr.CallStartGame();
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
        EventMgr.CallStartGame();
    }
}
