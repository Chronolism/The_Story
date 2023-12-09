using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMap : MonoBehaviour, IEnvironwentMap
{
    public string mapName { get ; set ; }

    public void OnEnter(Entity entity)
    {
        entity.AddBuff(4001, 20, entity);
    }

    public void OnExit(Entity entity)
    {
        entity.RemoveBuff(4001,20, entity);
    }

    public void OnUpdate(Entity entity)
    {
        
    }
}
