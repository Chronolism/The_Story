using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : BasePanel
{
    public override void Init()
    {
        
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnStartLocal":
                UIManager.Instance.ClearAllPanel();
                GameMgr.Instance.ChangeGameServerType(GameServerType.Local);
                UIManager.Instance.ShowPanel<SearchRoomPanel>();
                break;
            case "btnStartSteam":
                UIManager.Instance.ClearAllPanel();
                GameMgr.Instance.ChangeGameServerType(GameServerType.Steam);
                UIManager.Instance.ShowPanel<SearchRoomPanel>();
                break;
            case "btnQuit":
                Application.Quit();
                break;
        }
    }
}
