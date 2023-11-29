using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseMap 
{
    public void Init(MapTileDetileValue value);
    public MapTileDetileValue OnSave();
    public void OnOpenEditor(BaseMapEditorPanel editorPanel);
}
