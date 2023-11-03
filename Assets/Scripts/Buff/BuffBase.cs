using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuffBase 
{
    public string BuffID;
    public float Amount = 0;
    public float temporaryAmount;
    public float time;
    public Entity buffOwn;
    public BuffData buffData;
    public float cdMax;
    public float cd;
    public float energy;
    public float maxEnergy;
    public virtual void Init(string buffId,float amount,Entity buffOwn)
    {
        BuffID = buffId;
        Amount = amount;
        this.buffOwn = buffOwn;
        cd = cdMax;
    }
    public virtual void OnStart(Entity entity, float Value) { }
    public virtual void OnEnd(Entity entity, float Value) { }
    public virtual void OnAdd(Entity entity,float Value) { }
    public virtual void OnRemove(Entity entity, float Value) { }
    public virtual void OnChange(Entity entity, float Value) { }
    public virtual void OnTriger(Entity entity, float Value) { }
}
