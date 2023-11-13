using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class SteamMgr : SteamManager
{
    public static CSteamID lobbyID;
    protected override void Awake()
    {
        base.Awake();
        SteamAPI.RunCallbacks();
        //CallbackDispatcher
        SteamCallBack.Instance.OnGameConnectedFriendChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(OnGameConnectedFriendChatMsg);
        //当退出大厅
        SteamCallBack.Instance.OnLobbyKicked = Callback<LobbyKicked_t>.Create(OnLobbyKicked);
        //当大厅创建
        SteamCallBack.Instance.OnLobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        //当加入steam大厅
        SteamCallBack.Instance.OnLobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        //当大厅数据更新
        SteamCallBack.Instance.OnLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        /// SendLobbyChatMsg。这将向大厅内的所有用户发送一条简单的二进制消息。 
        /// 大厅成员需要听取 ISteamMatchmaking::LobbyChatMsg_t 回调。 
        /// 收到回调后，您可以使用 ISteamMatchmaking::GetLobbyChatEntry 获取消息内容
        SteamCallBack.Instance.OnLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        ///当收到邀请通知
        SteamCallBack.Instance.OnLobbyInvited = Callback<LobbyInvite_t>.Create(OnLobbyInvited);
        /// 如果用户已在游戏中，将提交 ISteamFriends::GameLobbyJoinRequested_t 回调，
        /// 其中包含了用户希望加入的大厅的 Steam ID。 这将由您的游戏决定是否接受。
        SteamCallBack.Instance.OnGameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        /// 如果目标用户接受了邀请，启动游戏时 pchConnectString 会添加入命令行。
        /// 如果该用户已在运行游戏，对方便会收到带有连接字符串的
        SteamCallBack.Instance.OnGameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);

        SteamCallBack.Instance.lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
    }

    private void OnLobbyMatchList(LobbyMatchList_t param)
    {
        List<SteamLobby> steamLobbyList = new List<SteamLobby>();
        for(int i = 0; i < param.m_nLobbiesMatching; i++)
        {
            Debug.Log(param);
            SteamLobby steamLobby = new SteamLobby();
            steamLobby.lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            Debug.Log(steamLobby.lobbyID);
            steamLobby.memberAmount = SteamMatchmaking.GetNumLobbyMembers(steamLobby.lobbyID);
            Debug.Log(steamLobby.memberAmount);
            steamLobby.roomName = SteamMatchmaking.GetLobbyData(steamLobby.lobbyID, "roomName");
            steamLobbyList.Add(steamLobby);
        }
        SeachLobbyCallBack?.Invoke(steamLobbyList);
    }

    private void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t param)
    {
        
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t param)
    {
        GameMgr.Instance.ChangeGameServerType(GameServerType.Steam);
        (MyNetworkManager.singleton as MyNetworkManager).JionRoom(new FriendRoom() { steamIP = param.m_steamIDLobby.m_SteamID });
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
        if (param.m_EChatRoomEnterResponse == (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            lobbyID = new CSteamID(param.m_ulSteamIDLobby);
        }
        JoinLobbyCallBack?.Invoke(param);
        JoinLobbyCallBack = null;
    }

    private void OnLobbyCreated(LobbyCreated_t param)
    {
        if (param.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError($" Create Fail {param.m_eResult}");
            CreatLobbyCallBack?.Invoke(param);
            return;
        }
        Debug.Log(" Create " + param.m_ulSteamIDLobby);
        Debug.Log("大厅创建");
        SteamMatchmaking.SetLobbyData(new CSteamID(param.m_ulSteamIDLobby), "name", "wmkj");
        SteamMatchmaking.SetLobbyData(new CSteamID(param.m_ulSteamIDLobby), "roomName", SteamFriends.GetPersonaName());
        lobbyID = new CSteamID(param.m_ulSteamIDLobby);
        CreatLobbyCallBack?.Invoke(param);
    }

    private void OnLobbyKicked(LobbyKicked_t param)
    {
        
    }

    /// <summary>
    /// 收到好友的消息
    /// </summary>
    /// <param name="param"></param>
    private void OnGameConnectedFriendChatMsg(GameConnectedFriendChatMsg_t param)
    {
        string pvDatas = "";
        EChatEntryType eChatEntryType;
        SteamFriends.GetFriendMessage(param.m_steamIDUser, param.m_iMessageID, out pvDatas, 8, out eChatEntryType);
        Debug.Log("收到来自[" + SteamFriends.GetFriendPersonaName(param.m_steamIDUser) + "]的消息!!===" + pvDatas);
        switch (pvDatas)
        {
            case "ChangedCustomMode":

                break;
        }
    }

    private static Action<LobbyEnter_t> JoinLobbyCallBack;
    public static void JoinLobby(ulong id ,Action<LobbyEnter_t> callback)
    {
        JoinLobbyCallBack = callback;
        SteamMatchmaking.JoinLobby(new CSteamID(id));
    }

    private static Action<LobbyCreated_t> CreatLobbyCallBack;
    public static void CreatLobby(Action<LobbyCreated_t> callBack)
    {
        CreatLobbyCallBack = callBack;
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 5);
    }

    private static Action<List<SteamLobby>> SeachLobbyCallBack;
    public static void SeachLobby(Action<List<SteamLobby>> callback)
    {
        SeachLobbyCallBack = callback;
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
        SteamMatchmaking.AddRequestLobbyListStringFilter("name", "wmkj", ELobbyComparison.k_ELobbyComparisonEqual);
        SteamMatchmaking.RequestLobbyList();
    }

    public static bool InvitedFriendToLobby(ulong id)
    {
        return SteamMatchmaking.InviteUserToLobby(lobbyID, new CSteamID(id));
    }

    public static List<SteamFriend> GetFriends()
    {
        List<SteamFriend> friends = new List<SteamFriend>();
        for (int i = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate); i > 0; i--)
        {
            CSteamID steamid = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            SteamFriend friend = new SteamFriend(SteamFriends.GetFriendPersonaName(steamid), steamid, default);
            friend.state = SteamFriends.GetFriendPersonaState(steamid);
            if (SteamFriends.GetFriendGamePlayed(steamid, out FriendGameInfo_t info))
            {
                friend.gameID = info.m_gameID;
            }
            friends.Add(friend);
        }
        return friends;
    }

    public static List<SteamFriend> GetOnLineFriend()
    {
        List<SteamFriend> friends = new List<SteamFriend>();
        foreach (SteamFriend friend in GetFriends())
        {
            if (friend.state == EPersonaState.k_EPersonaStateOnline)
            {
                friends.Add(friend);
            }
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
    public EPersonaState state;
    public SteamFriend() { }
    public SteamFriend(string name, CSteamID steamID, CGameID gameID)
    {
        this.name = name;
        this.steamID = steamID;
        this.gameID = gameID;
    }
}

public class SteamLobby
{
    public CSteamID lobbyID;
    public string roomName;
    public int memberAmount;
}