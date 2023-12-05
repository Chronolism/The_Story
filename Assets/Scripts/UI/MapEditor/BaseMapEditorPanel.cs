using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseMapEditorPanel : BasePanel
{
    public GameObject gbParent;
    [SerializeField] private GameObject TextCompenmet;
    [SerializeField] private GameObject FloatCompenmet;
    [SerializeField] private GameObject integerTrigerGUI;

    BaseMap baseMap;
    Vector3Int pos;

    Button btnQuit;

    public override void Init()
    {
        btnQuit = GetControl<Button>("btnQuit");
        btnQuit.onClick.AddListener(() =>
        {
            (parentPanel as MapEditorPanel).SaveTileValue(baseMap, pos);
            HidePanel(this);
        });
    }

    public void Init(BaseMap baseMap,Vector3Int pos)
    {
        this.baseMap = baseMap;
        this.pos = pos;
        baseMap.OnOpenEditor(this);
    }

    public void OnGUIText(string name , string content , UnityAction<string> callback)
    {
        GameObject gb = Instantiate(TextCompenmet, gbParent.transform);
        gb.GetComponent<TextInputField>().Init(name, content, callback);
    }

    public void OnGUIFloat(string name, float value, UnityAction<float> callback)
    {
        GameObject gb = Instantiate(FloatCompenmet, gbParent.transform);
        gb.GetComponent<FloatInputField>().Init(name, value, callback);
    }

    public void OnGUIIntegerTriger(string name, int value,UnityAction<int> callback)
    {
        GameObject gb = Instantiate(integerTrigerGUI, gbParent.transform);
        gb.GetComponent<IntegerTrigerGUI>().Init(name, value, callback);
    }
}
