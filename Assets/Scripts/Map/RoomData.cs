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

    public readonly SyncList<int> roomTags = new SyncList<int>();

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
        CharacterData character = DataMgr.Instance.GetCharacter(101);
        List<BuffDetile> skill = new List<BuffDetile>();
        skill.Add(character.skill_Index[0]);
        skill.Add(character.skill_Index[1]);
        RoomUserData roomUserData = new RoomUserData() { name = name, connectId = con.connectionId, characterId = 101, skills = skill, con = con };
        if (roomUser.ContainsKey(name)) return;
        roomUser.Add(name, roomUserData);
    }

    public void Awake()
    {
        DataMgr.Instance.roomData = this;
        roomLogic = new NormalRoom(this);
    }

    public void UpDataDetile(string oldvalue , string  value)
    {
        foreach(var a in observers)
        {
            a.ToUpdate(this);
        }
    }

    public void Update()
    {
        if (!ifPause)
        {
            roomLogic.Updata();
        }
    }
    /// <summary>
    /// 重载房间
    /// </summary>
    public void ReloadRoom()
    {
        roomLogic = new NormalRoom(this);
    }

    /// <summary>
    /// 呼叫服务器开始游戏
    /// </summary>
    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        //GameMgr.Instance.StartGame();
        roomLogic.StartGame();
        StartGameRpc();
    }
    /// <summary>
    /// 通知客户端开始游戏
    /// </summary>
    [ClientRpc]
    public void StartGameRpc()
    {
        roomLogic.StartGameClient();
    }
    /// <summary>
    /// 修改用户属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="roomUserData"></param>
    [Command(requiresAuthority = false)]
    public void ChangeRoomUserData(RoomUserData roomUserData)
    {
        roomUserData.con = roomUser[roomUserData.name].con;
        roomUser[roomUserData.name] = roomUserData;
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

        roomLogic.BeginGame();
        BeginGaneRPC();
        if (!isClient)EventMgr.CallStartGame();
        ifPause = false;
        
    }
    /// <summary>
    /// 客户端开始游戏
    /// </summary>
    [ClientRpc]
    public void BeginGaneRPC()
    {
        UIManager.Instance.ClearAllPanel();
        roomLogic.BeginGameClient();
        EventMgr.CallStartGame();
    }
    [Server]
    public void FinishGame()
    {
        roomLogic.FinishGame();
        FinishGameRpc();
    }
    [ClientRpc]
    public void FinishGameRpc()
    {
        roomLogic.FinishGameClient();
    }

    [Server]
    public void EndhGame()
    {
        roomLogic.EndGame();
        EndGameRpc();
    }
    [ClientRpc]
    public void EndGameRpc()
    {
        roomLogic.EndGameClient();
    }
}
