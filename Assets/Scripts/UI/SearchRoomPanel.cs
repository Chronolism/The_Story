using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class SearchRoomPanel : BasePanel
{
    ScrollRect srRoomList;
    public override void Init()
    {
        srRoomList = GetControl<ScrollRect>("srRoomList");
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnSearch":
                //for(int i =0; i < srRoomList.content.transform.childCount; i++)
                //{
                //    Destroy(srRoomList.content.transform.GetChild(i).gameObject);
                //}
                //foreach (var friends in SteamMgr.GetOnThisGameFriend())
                //{
                //    BtnRoom btnRoom = ResMgr.Instance.Load<GameObject>("UI/Compenent/btnRoom").GetComponent<BtnRoom>();
                //    btnRoom.Init(friends.steamID.m_SteamID, friends.name);
                //    btnRoom.transform.SetParent(srRoomList.content, false);
                //    btnRoom.btnRoom.onClick.AddListener(() =>
                //    {
                //        MyNetworkManager.singleton.networkAddress = btnRoom.steamID.ToString();
                //        MyNetworkManager.singleton.StartClient();
                //    });
                //}
                SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
                SteamMatchmaking.AddRequestLobbyListStringFilter("name", "wmkj", ELobbyComparison.k_ELobbyComparisonEqual);
                SteamMatchmaking.RequestLobbyList();
                break;
            case "btnHost":
                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 5);
                
                //MyNetworkManager.singleton.StartHost();
                break;

        }
    }



}
