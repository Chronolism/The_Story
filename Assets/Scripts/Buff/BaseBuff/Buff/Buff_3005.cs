public class Buff_3005 : BuffBase
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
		entity.AddBuff(1003, 100, Value, entity);
	}
}
