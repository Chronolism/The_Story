using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryMap : BaseMap, IEditorMap
{
    public float initAngle;
    public float maxAngle;
    public float maxDistance;
    public float minDistance;

    MapTileDetileValue value;

    public void Init(MapTileDetileValue value)
    {
        initAngle = value.value[0];
        maxAngle = value.value[1];
        maxDistance = value.value[2];
        minDistance = value.value[3];
        this.value = value;
    }

    public void OnOpenEditor(BaseMapEditorPanel editorPanel)
    {
        editorPanel.OnGUIFloat("��ʼ�Ƕ�", initAngle, (o) =>
        {
            initAngle = o;
        });
        editorPanel.OnGUIFloat("������ҽǶ�", maxAngle, (o) =>
        {
            maxAngle = o;
        });
        editorPanel.OnGUIFloat("������", maxDistance, (o) =>
        {
            maxDistance = o;
        });
        editorPanel.OnGUIFloat("��С����", minDistance, (o) =>
        {
            minDistance = o;
        });
    }

    public MapTileDetileValue OnSave()
    {
        return new MapTileDetileValue(initAngle, maxAngle, maxDistance, minDistance);
    }

    public override void OnServerStart()
    {
        EntityFactory.Instance.CreatEntity<ArtilleryBuild>(2003, transform.position).Init(value.value);
    }
}
