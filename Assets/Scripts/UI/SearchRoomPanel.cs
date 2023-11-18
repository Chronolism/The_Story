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
                for (int i = 0; i < srRoomList.content.transform.childCount; i++)
                {
                    Destroy(srRoomList.content.transform.GetChild(i).gameObject);
                }
                GameMgr.Instance.SearchRoom((o) =>
                {
                    foreach (var room in o)
                    {
                        BtnRoom btnRoom = ResMgr.Instance.Load<GameObject>("UI/Compenent/btnRoom").GetComponent<BtnRoom>();
                        btnRoom.Init(room, room.Name);
                        btnRoom.transform.SetParent(srRoomList.content, false);
                        btnRoom.btnRoom.onClick.AddListener(() =>
                        {
                            GameMgr.Instance.JoinRoom(room);
                        });
                    }
                });
                break;
            case "btnHost":
                GameMgr.Instance.CreatRoom();
                break;

        }
    }



}
