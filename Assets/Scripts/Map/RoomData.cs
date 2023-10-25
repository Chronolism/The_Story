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
    /// ���з�������ʼ��Ϸ
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
    /// �޸��û�����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="roomUserData"></param>
    [Command(requiresAuthority = false)]
    public void ChangeRoomUserData(string name ,RoomUserData roomUserData)
    {
        roomUser[name] = roomUserData;
    }

    /// <summary>
    /// ���������ص�ͼ
    /// </summary>
    /// <param name="name"></param>
    [Server]
    public void LoadMap(string name)
    {
        GameMgr.Instance.LoadMap(name);
        LoadMapRpc(name);
    }
    /// <summary>
    /// �ͻ��˼��ص�ͼ
    /// </summary>
    /// <param name="name"></param>
    [ClientRpc]
    public void LoadMapRpc(string name)
    {
        if (isServer) return;
        GameMgr.Instance.LoadMap(name);
    }
    /// <summary>
    /// ��������ʼ��Ϸ
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
    /// �ͻ��˿�ʼ��Ϸ
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
