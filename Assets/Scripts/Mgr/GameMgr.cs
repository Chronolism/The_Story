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
    /// �޸ķ�������
    /// </summary>
    /// <param name="gameServerType"></param>
    public void ChangeGameServerType(GameServerType gameServerType)
    {
        (MyNetworkManager.singleton as MyNetworkManager).ChangeGameServerType(gameServerType);
    }
    /// <summary>
    /// �������䣬��Name�����������������server����������Ͳ�ͬ������Ϊ�ա�
    /// </summary>
    /// <param name="callBack"></param>
    public void SearchRoom(UnityAction<List<FriendRoom>> callBack)
    {
        (MyNetworkManager.singleton as MyNetworkManager).SearchRoom(callBack);
    }
    /// <summary>
    /// ���뷿��
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
    /// ��������
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
                        EContent = "Steamδ����";
                        break;
                    case 16:
                        EContent = "Steamδ��Ӧ";
                        break;
                    case 2:
                        EContent = "Steam��Ӧ����";
                        break;
                    case 15:
                        EContent = "Steam���������ڸ���Ϸ��������";
                        break;
                    case 25:
                        EContent = "�㴴��̫��Ĵ���";
                        break;
                        default:
                        EContent = o.ToString();
                        break;
                }
                UIManager.Instance.ShowPanel<TipPanel>((p) => { p.SetCurrent("���䴴��ʧ��\n" + "����ԭ��" + EContent ,true); },true);
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>((o) => { o.SetCurrent("���䴴���ɹ�", true); }, true);
                MyNetworkManager.singleton.StartHost();
            }
           
        });


    }

    /// <summary>
    /// �޸ĵ�ͼ
    /// </summary>
    /// <param name="mapName"></param>
    public void ChangeMap(string mapName)
    {
        DataMgr.Instance.roomData.ChangeMap(mapName);
    }
}
