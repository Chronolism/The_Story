using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineModePanel : BasePanel
{
    public override void Init()
    {
        GetControl<Button>("ExitButton").onClick.AddListener(
            () => { HidePanel(this); });
        
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "PlayWithSteam":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Steam);
                UIManager.Instance.ShowPanel<LoadingPanel>((o) => {
                    o.AddWhileEnterCompletelyBlack(() =>
                    {       
                        UIManager.Instance.HidePanel<MainMenuPanel>();
                        UIManager.Instance.ShowPanel<LobbyPanel>();
                    });
                }, true);
                break;
            case "PlayWithLocal":
                GameMgr.Instance.ChangeGameServerType(GameServerType.Local);
                UIManager.Instance.ShowPanel<LoadingPanel>((o) => {
                    o.AddWhileEnterCompletelyBlack(() =>
                    {
                        UIManager.Instance.HidePanel<MainMenuPanel>();
                        UIManager.Instance.ShowPanel<LobbyPanel>();
                    });
                }, true);
                break;
            case "ExitButton":
                HidePanel(this);
                break;
        }
        
    }
}
