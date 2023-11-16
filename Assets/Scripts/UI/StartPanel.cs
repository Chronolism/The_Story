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
                GameMgr.Instance.ChangeGameServerType(GameServerType.Local);
                UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
                {
                    o.AddWhileEnterCompletelyBlack(() =>
                    {
                        UIManager.Instance.ClearAllPanel();
                        UIManager.Instance.ShowPanel<SearchRoomPanel>();
                    });
                }, true);
                break;
            case "btnStartSteam":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Steam);
                UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
                {
                    o.AddWhileEnterCompletelyBlack(() =>
                    {
                        UIManager.Instance.ClearAllPanel();
                        UIManager.Instance.ShowPanel<SearchRoomPanel>();
                    });
                }, true);
                break;
            case "btnQuit":
                Application.Quit();
                break;
        }
    }
}
