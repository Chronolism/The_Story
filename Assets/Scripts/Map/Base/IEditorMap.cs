using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditorMap 
{
    public void Init(MapTileDetileValue value);
    public MapTileDetileValue OnSave();
    public void OnOpenEditor(BaseMapEditorPanel editorPanel);
}
