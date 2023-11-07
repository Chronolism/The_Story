using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class SteamMgr : SteamManager
{
    protected override void Awake()
    {
        base.Awake();
        SteamAPI.RunCallbacks();
        //CallbackDispatcher
        SteamCallBack.Instance.OnGameConnectedFriendChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(OnGameConnectedFriendChatMsg);
        //���˳�����
        SteamCallBack.Instance.OnLobbyKicked = Callback<LobbyKicked_t>.Create(OnLobbyKicked);
        //����������
        SteamCallBack.Instance.OnLobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        //������steam����
        SteamCallBack.Instance.OnLobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        //���������ݸ���
        SteamCallBack.Instance.OnLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        /// SendLobbyChatMsg���⽫������ڵ������û�����һ���򵥵Ķ�������Ϣ�� 
        /// ������Ա��Ҫ��ȡ ISteamMatchmaking::LobbyChatMsg_t �ص��� 
        /// �յ��ص���������ʹ�� ISteamMatchmaking::GetLobbyChatEntry ��ȡ��Ϣ����
        SteamCallBack.Instance.OnLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        ///���յ�����֪ͨ
        SteamCallBack.Instance.OnLobbyInvited = Callback<LobbyInvite_t>.Create(OnLobbyInvited);
        /// ����û�������Ϸ�У����ύ ISteamFriends::GameLobbyJoinRequested_t �ص���
        /// ���а������û�ϣ������Ĵ����� Steam ID�� �⽫��������Ϸ�����Ƿ���ܡ�
        SteamCallBack.Instance.OnGameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        /// ���Ŀ���û����������룬������Ϸʱ pchConnectString ������������С�
        /// ������û�����������Ϸ���Է�����յ����������ַ�����
        SteamCallBack.Instance.OnGameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);
    }

    private void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t param)
    {
        
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t param)
    {
        
    }

    private void OnLobbyInvited(LobbyInvite_t param)
    {
        
    }

    private void OnLobbyChatMsg(LobbyChatMsg_t param)
    {
        
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t param)
    {
        
    }

    private void OnLobbyEnter(LobbyEnter_t param)
    {
        
    }

    private void OnLobbyCreated(LobbyCreated_t param)
    {
        
    }

    private void OnLobbyKicked(LobbyKicked_t param)
    {
        
    }

    /// <summary>
    /// �յ����ѵ���Ϣ
    /// </summary>
    /// <param name="param"></param>
    private void OnGameConnectedFriendChatMsg(GameConnectedFriendChatMsg_t param)
    {
        string pvDatas = "";
        EChatEntryType eChatEntryType;

        SteamFriends.GetFriendMessage(param.m_steamIDUser, param.m_iMessageID, out pvDatas, 8, out eChatEntryType);
        Debug.Log("�յ�����[" + SteamFriends.GetFriendPersonaName(param.m_steamIDUser) + "]����Ϣ!!===" + pvDatas);
        switch (pvDatas)
        {
            case "ChangedCustomMode":

                break;
        }
    }

    public static List<SteamFriend> GetFriends()
    {
        List<SteamFriend> friends = new List<SteamFriend>();
        for (int i = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate); i > 0; i--)
        {
            CSteamID steamid = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            SteamFriend friend = new SteamFriend(SteamFriends.GetFriendPersonaName(steamid), steamid, default);
            if (SteamFriends.GetFriendGamePlayed(steamid, out FriendGameInfo_t info))
            {
                friend.gameID = info.m_gameID;
            }
            friends.Add(friend);
        }
        return friends;
    }

    public static List<SteamFriend> GetOnThisGameFriend()
    {
        List<SteamFriend> friends = new List<SteamFriend>();
        foreach (SteamFriend friend in GetFriends())
        {
            if (friend.gameID.m_GameID == DataMgr.Instance.steamAppID)
            {
                friends.Add(friend);
            }
        }
        return friends;
    }

}

public class SteamFriend
{
    public string name;
    public CSteamID steamID;
    public CGameID gameID;
    public SteamFriend() { }
    public SteamFriend(string name, CSteamID steamID, CGameID gameID)
    {
        this.name = name;
        this.steamID = steamID;
        this.gameID = gameID;
    }
}