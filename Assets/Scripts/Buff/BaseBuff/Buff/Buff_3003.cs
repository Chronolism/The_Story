public class Buff_3003 : BuffBase
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
		entity.Atttack(1001, new UnityEngine.Vector3(Value, 0, 0));
	}
}
