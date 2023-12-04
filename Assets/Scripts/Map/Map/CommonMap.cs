using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMap : MonoBehaviour,BaseEnvironwentMap
{
    public string mapName { get { return buff.buffId.ToString(); }}

    public BuffDetile buff;

    public void OnEnter(Entity entity)
    {
        entity.AddBuff(buff.buffId, buff.buffValue, entity);
    }

    public void OnExit(Entity entity)
    {
        entity.RemoveBuff(buff.buffId, buff.buffValue, entity);
    }

    public void OnUpdate(Entity entity)
    {
        
    }

}
