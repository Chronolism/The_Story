public class Buff_3001 : BuffBase
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
		foreach(var servitor in entity.entityServitor.Servitors)
		{
			servitor.AddBuff(1001, 10, Value, entity);
			servitor.AddBuff(1003, 1, Value, entity);
		}
	}
}
