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
        //UIManager.Instance.ShowPanel<RoomPanel>();
        EntityFactory.Instance.CreatRoomData();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        transport.OnClientDisconnected = () => { Debug.Log("���������Ӳ���"); };
        
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        //DataMgr.Instance.roomData.AddRoomUser(DataMgr.Instance.playerData.account);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log(conn.address + "�����Ϸ�����");
        
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("�������Ͽ�");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        //if (NetworkServer.active) return;
        UIManager.Instance.ClearAllPanel();
        UIManager.Instance.ShowPanel<RoomPanel>();
        NetworkClient.Send(new C2S_JionRoom() { name = DataMgr.Instance.playerData.account });
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    }

    
}
