

using UnityEngine;

public class Buff_1003 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		

    }

	public override void OnEnd(Entity entity,float Value)
	{
        
    }
    public override void OnAdd(Entity entity, float Value)
    {
        entity.maxSpeed += Value/100;
    }
    public override void OnRemove(Entity entity, float Value)
    {
        entity.maxSpeed -= Value/100;
    }

    public override void OnAddEffect(Entity entity, float Value)
    {
        entity.GetComponent<EntityEffect>().ShowEffectOnClient(1003);
    }

    public override void OnRemoveEffect(Entity entity, float Value)
    {
        entity.GetComponent<EntityEffect>().HideEffectOnClient(1003);
    }
}
