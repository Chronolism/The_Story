using Mirror;
using Mirror.Discovery;
#if UNITY_STANDALONE_WIN
using Steamworks;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using TheStory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MyNetworkManager : NetworkManager
{
    public Transport local;
    public Transport steam;
    public GameServerType gameServerType = GameServerType.Local;
    public NetworkDiscovery networkDiscovery;
    int tryJoinTimes = 5;
    public MsgPool msgPool;
    public override void Awake()
    {
        base.Awake();
        msgPool = new MsgPool();
        foreach (var i in DataMgr.Instance.AttackDatas)
        {
            spawnPrefabs.Add(i.gameObject);
        }
        networkDiscovery = local.GetComponent<NetworkDiscovery>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //UIManager.Instance.ShowPanel<RoomPanel>();
        EntityFactory.Instance.CreatRoomData();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        transport.OnClientDisconnected = () => { 
            Debug.Log("服务器连接不上");
            StopClient();
        };
        
        
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        //DataMgr.Instance.roomData.AddRoomUser(DataMgr.Instance.playerData.account);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log(conn.address + "连接上服务器");
        
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("服务器断开");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        //if (NetworkServer.active) return;
        tryJoinTimes = 0;
        GameCallBack.Instance.SuccesJionRoom?.Invoke();
        //UIManager.Instance.ClearAllPanel();
        //UIManager.Instance.ShowPanel<RoomPanel>();
        NetworkClient.Send(new C2S_JionRoom() { name = DataMgr.Instance.playerData.account });
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("DisConnect");
        if(gameServerType == GameServerType.Steam)
        {
            tryJoinTimes--;
            if(tryJoinTimes >= 0)
            {
                StartClient();
                return;
            }
            SteamMgr.StopClient();
        }
        UIManager.Instance.ShowPanel<TipPanel>((p) =>
        {
            p.SetCurrent("断开连接或连接不上\n", true);
        });
    }

    public void ChangeGameServerType(GameServerType gameServerType)
    {
        this.gameServerType = gameServerType;
        switch (gameServerType)
        {
            case GameServerType.Local:
                transport = local;
                Transport.active = local;
                break;
            case GameServerType.Steam:
                transport = steam;
                Transport.active = steam;
                break;
        }
    }

    public void SearchRoom(UnityAction<List<FriendRoom>> callback)
    {
        StartCoroutine(ISearchRoom(callback));
        
    }

    IEnumerator ISearchRoom(UnityAction<List<FriendRoom>> callback)
    {
        List<FriendRoom> friendRooms = new List<FriendRoom>();
        switch (gameServerType)
        {
            case GameServerType.Local:
                networkDiscovery.StartDiscovery();
                networkDiscovery.OnServerFound.AddListener((o) => 
                    { 
                        friendRooms.Add(new FriendRoom() { Name = o.EndPoint.Address.ToString(), localIP = o }); 
                    });
                yield return new WaitForSeconds(2);
                Debug.Log("找到" + friendRooms.Count + "房间");
                networkDiscovery.OnServerFound.RemoveAllListeners();
                networkDiscovery.StopDiscovery();
                callback?.Invoke(friendRooms);
                break;
            case GameServerType.Steam:
                SteamMgr.SeachLobby((o) =>
                {
                    foreach (var lobby in o)
                    {
                        friendRooms.Add(new FriendRoom() { Name = lobby.roomName, steamIP = lobby.lobbyID.m_SteamID });
                    }
                    callback?.Invoke(friendRooms);
                });
                break;
        }
        
    }

    public void JionRoom(FriendRoom room)
    {
        switch (gameServerType)
        {
            case GameServerType.Local:
                StartClient(room.localIP.uri);
                break;
#if UNITY_STANDALONE_WIN
            case GameServerType.Steam:
                SteamMgr.JoinLobby(room.steamIP, (o) =>
                {
                    if (o.m_EChatRoomEnterResponse != (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
                    {
                        UIManager.Instance.ShowPanel<TipPanel>((p) => 
                        {
                            p.SetCurrent("加入失败\n失败原因：" + (EChatRoomEnterResponse)o.m_EChatRoomEnterResponse, true);
                        });
                    }
                    else
                    {
                        networkAddress = SteamMatchmaking.GetLobbyOwner(new CSteamID(o.m_ulSteamIDLobby)).ToString();
                        tryJoinTimes = 5;
                        StartClient();
                    }
                });
                break;
#endif
        }
    }

    public void QuitRoom()
    {
        NetworkClient.Send(new C2S_QuitRoom() { name = DataMgr.Instance.playerData.account });
        StopClient();
    }

    public void CreatRoom(Action<int> callBack)
    { 
        switch (gameServerType)
        {
            case GameServerType.Local:
                callBack?.Invoke(1);
                networkDiscovery.AdvertiseServer();
                break;
#if UNITY_STANDALONE_WIN
            case GameServerType.Steam:
                UIManager.Instance.ShowPanel<TipPanel>((o) => { o.SetCurrent("房间创建中"); });
                SteamMgr.CreatLobby(callBack);
                break;
#endif
        }
    }

    public void InvitedSteamFriendToLobby(ulong id)
    {
        if (!SteamMgr.InvitedFriendToLobby(id))
        {
            UIManager.Instance.ShowPanel<TipPanel>((p) =>
            {
                p.SetCurrent("用户无法邀请", true);
            });
        }
    }
}

public enum GameServerType
{
    Local,
    Steam
}