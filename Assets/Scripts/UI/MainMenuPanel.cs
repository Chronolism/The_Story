using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public override void Init()
    {
        GetControl<Button>("OfflinePlayButton").onClick.AddListener(OfflinePlayButtonAction);
        GetControl<Button>("OnlinePlayButton").onClick.AddListener(OnlinePlayButtonAction);
    }

    void OfflinePlayButtonAction()
    {
        UIManager.Instance.ShowPanel<LoadingPanel>("SystemLayer",(o) => {
            o.AddWhileEnterCompletelyBlack(() => 
        {
            UIManager.Instance.HidePanel<MainMenuPanel>();
            UIManager.Instance.ShowPanel<OfflineRoomPanel>("GameLayer");
        });});
    }
    void OnlinePlayButtonAction()
    {
        UIManager.Instance.ShowAsFloatingPanel<OnlineModePanel>(this,"GameLayer");
    }
}
