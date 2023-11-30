using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseMapEditorPanel : BasePanel
{
    public GameObject gbParent;
    [SerializeField] private GameObject TextCompenmet;

    Button btnQuit;

    public override void Init()
    {
        btnQuit = GetControl<Button>("btnQuit");
        btnQuit.onClick.AddListener(() =>
        {
            HidePanel(this);
        });
    }


    public void OnGUIText(string name , string content , UnityAction<string> callback)
    {
        Debug.Log("¿ªÊ¼»æÖÆ");
        GameObject gb = Instantiate(TextCompenmet, gbParent.transform);

        foreach(Text text in gb.GetComponentsInChildren<Text>())
        {
            if(text.name == "txtName")
            {
                text.text = name;
                return;
            }
        }

        InputField inputField = gb.GetComponentInChildren<InputField>();
        inputField.text = content;
        inputField.onValueChanged.AddListener((o) =>
        {
            callback?.Invoke(o);
        });
    }
}
