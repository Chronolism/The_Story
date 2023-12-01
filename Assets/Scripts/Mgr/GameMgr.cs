using Mirror;
//using Steamworks;
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
        //MapManager.Instance.LoadMapCompletelyToScene(name);

        //AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
        MapMgr.Instance.LoadMap(name);
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

    public void QuitRoom()
    {
        UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
        {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                (MyNetworkManager.singleton as MyNetworkManager).QuitRoom();
                UIManager.Instance.ClearAllPanel();
                UIManager.Instance.ShowPanel<MainMenuPanel>();
            });
        }, true);

    }
    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreatRoom()
    {
        (MyNetworkManager.singleton as MyNetworkManager).CreatRoom((o) => 
        {
            if (o != 1) 
            {
                // k_EResultNoConnection - your Steam client doesn't have a connection to the back-end
                // k_EResultTimeout - you the message to the Steam servers, but it didn't respond
                // k_EResultFail - the server responded, but with an unknown internal error
                // k_EResultAccessDenied - your game isn't set to allow lobbies, or your client does haven't rights to play the game
                // k_EResultLimitExceeded - your game client has created too many lobbies
                string EContent;
                switch (o)
                {
                    case 3:
                        EContent = "Steam未连接";
                        break;
                    case 16:
                        EContent = "Steam未响应";
                        break;
                    case 2:
                        EContent = "Steam相应出错";
                        break;
                    case 15:
                        EContent = "Steam不允许你在该游戏创建大厅";
                        break;
                    case 25:
                        EContent = "你创建太多的大厅";
                        break;
                        default:
                        EContent = o.ToString();
                        break;
                }
                UIManager.Instance.ShowPanel<TipPanel>((p) => { p.SetCurrent("房间创建失败\n" + "错误原因：" + EContent ,true); },true);
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>((o) => { o.SetCurrent("房间创建成功", true); }, true);
                MyNetworkManager.singleton.StartHost();
            }
           
        });


    }

    /// <summary>
    /// 修改地图
    /// </summary>
    /// <param name="mapName"></param>
    public void ChangeMap(string mapName)
    {
        DataMgr.Instance.roomData.ChangeMap(mapName);
    }
}
