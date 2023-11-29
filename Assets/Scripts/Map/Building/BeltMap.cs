using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltMap : MonoBehaviour,BaseMap
{
    public float speed;

    public void Init(MapTileDetileValue value)
    {
        speed = value.value[0];
    }

    public void OnOpenEditor(BaseMapEditorPanel editorPanel)
    {
        editorPanel.OnGUIText("speed", speed.ToString(), (o) => { speed = float.Parse(o); });
    }

    public MapTileDetileValue OnSave()
    {
        return new MapTileDetileValue() { value = new List<float>() { speed} };
    }
}
