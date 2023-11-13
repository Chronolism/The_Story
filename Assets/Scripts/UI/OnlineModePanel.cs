using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineModePanel : BasePanel
{
    public override void Init()
    {
       
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "PlayWithSteam":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Steam);
                break;
            case "PlayWithLocal":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Local);
                break;
            case "ExitButton":
                UIManager.Instance.HidePanel<OnlineModePanel>();
                break;
        }
        UIManager.Instance.ShowPanel<LoadingPanel>("SystemLayer", (o) => {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                UIManager.Instance.HidePanel<MainMenuPanel>();
                UIManager.Instance.HidePanel<OnlineModePanel>();
                UIManager.Instance.ShowPanel<LobbyPanel>("GameLayer");
            });
        });
    }
}
