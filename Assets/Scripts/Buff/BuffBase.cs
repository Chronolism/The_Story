using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase 
{
    public float Range = 1;
    public int Amount = 0;
    public virtual void OnStart(Entity entity, int Value) { }
    public virtual void OnEnd(Entity entity, int Value) { }
    public virtual void OnAdd(Entity entity,int Value) { }
    public virtual void OnRemove(Entity entity, int Value) { }
    public virtual void OnChange(Entity entity, int Value) { }
    public virtual void OnTriger(Entity entity, int Value) { }
}
