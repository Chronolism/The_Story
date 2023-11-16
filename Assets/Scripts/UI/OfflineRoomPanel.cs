using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineRoomPanel :BasePanel
{
    public override void Init()
    {
        GetControl<Button>("ExitButton").onClick.AddListener(BackToTitle);
        GetControl<Button>("StartButton").onClick.AddListener(StartGame);
    }

    void BackToTitle()
    {
        UIManager.Instance.ShowPanel<LoadingPanel>((o) => {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                UIManager.Instance.HidePanel<OfflineRoomPanel>();
                UIManager.Instance.ShowPanel<MainMenuPanel>();
            });
        }, true);
    }
    void StartGame()
    {

    }
}
