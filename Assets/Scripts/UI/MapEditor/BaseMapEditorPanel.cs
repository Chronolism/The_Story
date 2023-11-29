using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseMapEditorPanel : BasePanel
{
    public GameObject gbParent;
    [SerializeField] private GameObject TextCompenmet;
    public override void Init()
    {
        
    }


    public void OnGUIText(string name , string content , UnityAction<string> callback)
    {
        GameObject gb = Instantiate(TextCompenmet, this.transform);

        gb.GetComponentInChildren<Text>().text = name;
        InputField inputField = gb.GetComponentInChildren<InputField>();
        inputField.text = content;
        inputField.onValueChanged.AddListener((o) =>
        {
            callback?.Invoke(o);
        });
    }
}
