using UnityEngine;

public class Buff_3002 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
	}
	public override void OnEnd(Entity entity,float Value)
	{
	}
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}

	public override void OnTriger(Entity entity, float Value)
	{
		entity.AddBuff(1001, 100, Value, entity);
	}
}
