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
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UIManager.Instance.ShowPanel<RoomPanel>();
        NetworkClient.Send(new C2S_JionRoom() { name = DataMgr.Instance.playerData.account });
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        UIManager.Instance.ShowPanel<RoomPanel>();
        RoomData roomData = EntityFactory.Instance.CreatRoomData();
        Debug.Log(roomData);
        roomData.AddRoomUser(DataMgr.Instance.playerData.account);
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
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    }
}
