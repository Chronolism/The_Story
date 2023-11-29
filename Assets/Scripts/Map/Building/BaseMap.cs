using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseMap 
{
    public void Init(object value);
    public void OnSave(out object value);
    public void OnOpenEditor(BaseMapEditorPanel editorPanel);
}
