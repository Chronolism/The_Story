using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase 
{
    public string BuffID;
    public float Amount = 0;
    public float time;
    public Entity buffOwn;
    public BuffData buffData;
    public virtual void OnStart(Entity entity, float Value) { }
    public virtual void OnEnd(Entity entity, float Value) { }
    public virtual void OnAdd(Entity entity,float Value) { }
    public virtual void OnRemove(Entity entity, float Value) { }
    public virtual void OnChange(Entity entity, float Value) { }
    public virtual void OnTriger(Entity entity, float Value) { }
}
