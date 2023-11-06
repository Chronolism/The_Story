public class Buff_3004 : BuffBase
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
		foreach(Servitor servitor in entity.Servitors)
		{
			servitor.AddBuff(1004, 40, Value, entity);
			servitor.AddBuff(1013, 1, Value, entity);
		}
	}
}
