using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public override void Init()
    {
        
    }

    public void SetCurrent(string content,bool ifShowButton = false,string btnName = "È·¶¨",UnityAction callback = null)
    {
        GetControl<Text>("txtContent").text = content;
        if (ifShowButton)
        {
            GetControl<Text>("txtSure").text = btnName;
            GetControl<Button>("btnSure").onClick.AddListener(() => 
            {
                UIManager.Instance.HidePanel<TipPanel>(); 
                callback?.Invoke();
            });
        }
    }
}
