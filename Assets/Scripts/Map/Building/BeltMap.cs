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
        editorPanel.OnGUIFloat("speed", speed, (o) => { speed = o; });
    }

    public MapTileDetileValue OnSave()
    {
        return new MapTileDetileValue() { value = new List<float>() { speed} };
    }
}
