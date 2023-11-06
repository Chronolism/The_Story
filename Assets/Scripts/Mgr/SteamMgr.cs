using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class SteamMgr : BaseManager<SteamMgr>
{
    public IntPtr Self;
    public SteamMgr()
    {
        Self = GetUserInterfacePointer();
    }

    public List<SteamFriend> GetFriends()
    {
        List<SteamFriend> friends = new List<SteamFriend>();
        for (int i = GetFriendCount(0x04); i > 0; i--)
        {
            ulong steamid = GetFriendByIndex(i, 0x04);
            SteamFriend friend = new SteamFriend(GetFriendPersonaName(steamid), steamid, 0);
            GameInfo info = default(GameInfo);
            if (GetFriendGamePlayed(steamid, ref info))
            {
                friend.gameID = info.GameID;
            }
            friends.Add(friend);
        }
        return friends;
    }

    public List<SteamFriend> GetOnThisGameFriend()
    {
        List<SteamFriend> friends = new List<SteamFriend>();
        foreach (SteamFriend friend in GetFriends())
        {
            if(friend.gameID == DataMgr.Instance.steamAppID)
            {
                friends.Add(friend);
            }
        }
        return friends;
    }

    [DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr SteamAPI_SteamFriends_v017();
    public virtual IntPtr GetUserInterfacePointer()
    {
        return SteamAPI_SteamFriends_v017();
    }

    [DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCount")]
    private static extern int _GetFriendCount(IntPtr self, int iFriendFlags);

    internal int GetFriendCount(int iFriendFlags)
    {
        return _GetFriendCount(Self, iFriendFlags);
    }

    [DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendByIndex")]
    private static extern ulong _GetFriendByIndex(IntPtr self, int iFriend, int iFriendFlags);

    internal ulong GetFriendByIndex(int iFriend, int iFriendFlags)
    {
        return _GetFriendByIndex(Self, iFriend, iFriendFlags);
    }

    [DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaName")]
    private static extern IntPtr _GetFriendPersonaName(IntPtr self, ulong steamid);

    internal unsafe string GetFriendPersonaName(ulong steamid)
    {
        IntPtr ptr = _GetFriendPersonaName(Self, steamid);
        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        byte* bptr = (byte*)(void*)ptr;
        int i;
        for (i = 0; i < 67108864 && bptr[i] != 0; i++)
        {
        }

        return Encoding.UTF8.GetString(bptr, i);
    }

    [DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendGamePlayed")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool _GetFriendGamePlayed(IntPtr self, ulong steamIDFriend, ref GameInfo pFriendGameInfo);

    internal bool GetFriendGamePlayed(ulong steamIDFriend, ref GameInfo pFriendGameInfo)
    {
        return _GetFriendGamePlayed(Self, steamIDFriend, ref pFriendGameInfo);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct GameInfo
    {
        internal ulong GameID;

        internal uint GameIP;

        internal ushort GamePort;

        internal ushort QueryPort;

        internal ulong SteamIDLobby;
    }
}

public class SteamFriend
{
    public string name;
    public ulong steamID;
    public ulong gameID;
    public SteamFriend() { }
    public SteamFriend(string name, ulong steamID, ulong gameID)
    {
        this.name = name;
        this.steamID = steamID;
        this.gameID = gameID;
    }
}