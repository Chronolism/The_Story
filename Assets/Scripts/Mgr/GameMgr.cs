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
    /// <summary>
    /// 修改服务类型
    /// </summary>
    /// <param name="gameServerType"></param>
    public void ChangeGameServerType(GameServerType gameServerType)
    {
        (MyNetworkManager.singleton as MyNetworkManager).ChangeGameServerType(gameServerType);
    }
    /// <summary>
    /// 搜索房间，除Name以外变量，用于连接server，因服务类型不同，可能为空。
    /// </summary>
    /// <param name="callBack"></param>
    public void SearchRoom(UnityAction<List<FriendRoom>> callBack)
    {
        (MyNetworkManager.singleton as MyNetworkManager).SearchRoom(callBack);
    }
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="friendRoom"></param>
    public void JoinRoom(FriendRoom friendRoom)
    {
        (MyNetworkManager.singleton as MyNetworkManager).JionRoom(friendRoom);
    }
    /// <summary>
    /// 创建房间
    /// </summary>
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
