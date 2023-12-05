using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltMap : MonoBehaviour,BaseMap
{
    public AreaEffector2D effector;
    public float speed;
    public float angle;

    public void Init(MapTileDetileValue value)
    {
        speed = value.value[0];
        angle = value.value[1];
        effector.forceAngle = angle;
        effector.forceVariation = speed * 300;
    }


    public void OnOpenEditor(BaseMapEditorPanel editorPanel)
    {
        editorPanel.OnGUIFloat("speed", speed, (o) => { speed = o; });
        editorPanel.OnGUIFloat("angle", angle, (o) => { angle = o; });
    }

    public MapTileDetileValue OnSave()
    {
        return new MapTileDetileValue() { value = new List<float>() { speed , angle} };
    }
}
