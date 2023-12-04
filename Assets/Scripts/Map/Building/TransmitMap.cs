using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitMap : MonoBehaviour, BaseMap
{
    public Vector3 pos;

    public void Init(MapTileDetileValue value)
    {
        pos = new Vector3(value.value[0], value.value[1]);
    }

    public void OnOpenEditor(BaseMapEditorPanel editorPanel)
    {
        editorPanel.OnGUIFloat("x", pos.x, (o) => { pos.x = o; });
        editorPanel.OnGUIFloat("y", pos.y, (o) => { pos.y = o; });
    }

    public MapTileDetileValue OnSave()
    {
        return new MapTileDetileValue() { value = new List<float>() { pos.x, pos.y } };
    }



}
