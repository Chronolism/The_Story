using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMgr : BaseManager<GameMgr>
{

    public void StartGame()
    {
        
        
    }

    public void StopGame()
    {

    }

    public void LoadMap(string name)
    {
        MapManager.Instance.LoadMapCompletelyToScene(name);

        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
    }

    public void ChangeGameServerType(GameServerType gameServerType)
    {
        (MyNetworkManager.singleton as MyNetworkManager).ChangeGameServerType(gameServerType);
    }
    
    public void SearchRoom(UnityAction<List<FriendRoom>> callBack)
    {
        (MyNetworkManager.singleton as MyNetworkManager).SearchRoom(callBack);
    }

    public void JoinRoom(FriendRoom friendRoom)
    {
        (MyNetworkManager.singleton as MyNetworkManager).JionRoom(friendRoom);
    }

    public void CreatRoom()
    {
        (MyNetworkManager.singleton as MyNetworkManager).CreatRoom((o) => 
        {
            if (o.m_eResult != EResult.k_EResultOK) 
            {
                UIManager.Instance.ClearAllPanel();
                UIManager.Instance.ShowPanel<StartPanel>();
            }
            else
            {
                MyNetworkManager.singleton.StartHost();
            }
        });
        
    }

}
