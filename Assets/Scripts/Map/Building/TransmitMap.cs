using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitMap : MonoBehaviour, BaseMap
{
    public Vector3 pos;

    public void Init(MapTileDetileValue value)
    {
        pos = new Vector3(value.value[0] + 0.5f, value.value[1] + 0.5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EntityEnvironment>(out EntityEnvironment environment))
        {
            environment.entity.gameObject.transform.position = pos;
        }
    }

}
