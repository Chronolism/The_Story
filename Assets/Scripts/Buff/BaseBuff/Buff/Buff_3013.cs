public class Buff_3013 : BuffBase
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
		entity.RewriteServitor(EntityFactory.Instance.CreatServitor(entity.transform.position, entity.ifPause));
	}
}
