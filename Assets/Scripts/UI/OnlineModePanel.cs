using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineModePanel : BasePanel
{
    public override void Init()
    {
        GetControl<Button>("ExitButton").onClick.AddListener(
            () => { UIManager.Instance.HidePanel<OnlineModePanel>(); });
        
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "PlayWithSteam":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Steam);
                UIManager.Instance.ShowPanel<LoadingPanel>("SystemLayer", (o) => {
                    o.AddWhileEnterCompletelyBlack(() =>
                    {
                        UIManager.Instance.HidePanel<MainMenuPanel>();
                        UIManager.Instance.HidePanel<OnlineModePanel>();
                        UIManager.Instance.ShowPanel<LobbyPanel>("GameLayer");
                    });
                });
                break;
            case "PlayWithLocal":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Local);
                UIManager.Instance.ShowPanel<LoadingPanel>("SystemLayer", (o) => {
                    o.AddWhileEnterCompletelyBlack(() =>
                    {
                        UIManager.Instance.HidePanel<MainMenuPanel>();
                        UIManager.Instance.HidePanel<OnlineModePanel>();
                        UIManager.Instance.ShowPanel<LobbyPanel>("GameLayer");
                    });
                });
                break;
            case "ExitButton":
                UIManager.Instance.HidePanel<OnlineModePanel>();
                break;
        }
        
    }
}
