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
        GameCallBack.Instance.SuccesJionRoom = () => { UIManager.Instance.ClearAllPanel(); };
    }
    /// <summary>
    /// ��������
    /// </summary>
    public void CreatRoom()
    {
        UIManager.Instance.ShowPanel<TipPanel>((o) => { o.SetCurrent("���䴴����"); });
        (MyNetworkManager.singleton as MyNetworkManager).CreatRoom((o) => 
        {
            if (o.m_eResult != EResult.k_EResultOK) 
            {
                // k_EResultNoConnection - your Steam client doesn't have a connection to the back-end
                // k_EResultTimeout - you the message to the Steam servers, but it didn't respond
                // k_EResultFail - the server responded, but with an unknown internal error
                // k_EResultAccessDenied - your game isn't set to allow lobbies, or your client does haven't rights to play the game
                // k_EResultLimitExceeded - your game client has created too many lobbies
                string EContent;
                switch (o.m_eResult)
                {
                    case EResult.k_EResultNoConnection:
                        EContent = "Steamδ����";
                        break;
                    case EResult.k_EResultTimeout:
                        EContent = "Steamδ��Ӧ";
                        break;
                    case EResult.k_EResultFail:
                        EContent = "Steam��Ӧ����";
                        break;
                    case EResult.k_EResultAccessDenied:
                        EContent = "Steam���������ڸ���Ϸ��������";
                        break;
                    case EResult.k_EResultLimitExceeded:
                        EContent = "�㴴��̫��Ĵ���";
                        break;
                        default:
                        EContent = o.m_eResult.ToString();
                        break;
                }
                UIManager.Instance.ShowPanel<TipPanel>((p) => { p.SetCurrent("���䴴��ʧ��\n" + "����ԭ��" + EContent ,true); });
            }
            else
            {
                GameCallBack.Instance.SuccesJionRoom = () => { UIManager.Instance.ClearAllPanel(); };
                MyNetworkManager.singleton.StartHost();              
            }
        });
        
    }

}
