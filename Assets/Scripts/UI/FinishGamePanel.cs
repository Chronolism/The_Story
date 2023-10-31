using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGamePanel : BasePanel
{

    public override void Init()
    {
        if(DataMgr.Instance.roomData.HostUser == DataMgr.Instance.playerData.account)
        {
            GetControl<Button>("btnNext").gameObject.SetActive(true);
            GetControl<Text>("txtContent").text = "开启下一句";
        }
        else
        {
            GetControl<Button>("btnNext").gameObject.SetActive(false);
            GetControl<Text>("txtContent").text = "等待房主开启下一句";
        }
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnNext":
                DataMgr.Instance.roomData.StartGame();
                break;
        }
    }
}
