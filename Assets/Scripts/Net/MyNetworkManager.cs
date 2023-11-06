using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    public override void Awake()
    {
        base.Awake();
        MessageRegister.Instance.RegisterMessage();
        foreach(var i in DataMgr.Instance.AttackDatas)
        {
            spawnPrefabs.Add(i.gameObject);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        UIManager.Instance.ShowPanel<RoomPanel>();
        EntityFactory.Instance.CreatRoomData();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (NetworkServer.active) return;
        UIManager.Instance.ShowPanel<RoomPanel>();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        //DataMgr.Instance.roomData.AddRoomUser(DataMgr.Instance.playerData.account);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        //if (NetworkServer.active) return;
        NetworkClient.Send(new C2S_JionRoom() { name = DataMgr.Instance.playerData.account });
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    }
}
