using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel,Observer<RoomData>
{
    public RoomData roomData;

    public Text txtUserList;
    public override void Init()
    {
        txtUserList = GetControl<Text>("txtUserList");
    }

    public void InitData(RoomData roomData)
    {
        this.roomData = roomData;
        roomData.roomUser.Callback += (a, b, c) =>
        {
            txtUserList.text = "";
            foreach (var user in roomData.roomUser) { txtUserList.text += user.Key + "\n"; }
        };
    }

    public void ToUpdate(RoomData value)
    {
        
    }
}
