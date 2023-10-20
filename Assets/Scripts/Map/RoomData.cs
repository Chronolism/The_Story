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
    /// ���з�������ʼ��Ϸ
    /// </summary>
    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        GameMgr.Instance.StartGame();
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
    /// ����������ȫ����ɫ�������Ȩ��
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
    /// ��������ʼ��Ϸ
    /// </summary>
    [Server]
    public void BeginGane()
    {
        UIManager.Instance.ClearAllPanel();
        EventMgr.CallStartGame();
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
        EventMgr.CallStartGame();
    }
}
