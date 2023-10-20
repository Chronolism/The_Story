using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel,Observer<RoomData>
{
    public RoomData roomData;

    public Text txtUserList;
    public Button btnStart;
    public override void Init()
    {
        txtUserList = GetControl<Text>("txtUserList");
        btnStart = GetControl<Button>("btnStart");
        StartCoroutine(findRoomData());
    }

    public void InitData(RoomData roomData)
    {
        this.roomData = roomData;
        txtUserList.text = "";
        foreach (var user in roomData.roomUser) { txtUserList.text += user.Key + "\n"; }
        roomData.roomUser.Callback += (a, b, c) =>
        {
            txtUserList.text = "";
            foreach (var user in roomData.roomUser) { txtUserList.text += user.Key + "\n"; }
            if (roomData.HostUser == DataMgr.Instance.playerData.account)
            {
                btnStart.gameObject.SetActive(true);
            }
            else
            {
                btnStart.gameObject.SetActive(false);
            }
        };
        if(roomData.HostUser == DataMgr.Instance.playerData.account)
        {
            btnStart.gameObject.SetActive(true);
        }
        else
        {
            btnStart.gameObject.SetActive(false);
        }
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnStart":
                roomData.StartGame();
                break;
        }
    }


    public void ToUpdate(RoomData value)
    {
        
    }

    IEnumerator findRoomData()
    {
        while(DataMgr.Instance.roomData == null)
        {
            yield return null;
        }
        InitData(DataMgr.Instance.roomData);
    }
}
