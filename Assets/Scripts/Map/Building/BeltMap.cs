using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltMap : MonoBehaviour,BaseMap
{
    public float speed;

    public void Init(object value)
    {
        speed = (float)value;
    }

    public void OnOpenEditor(BaseMapEditorPanel editorPanel)
    {
        editorPanel.OnGUIText("speed", speed.ToString(), (o) => { speed = float.Parse(o); });
    }

    public void OnSave(out object value)
    {
        value = speed;
    }
}
