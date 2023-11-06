public class Buff_3009 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnGetInk += LessInkGet;

    }
	public override void OnEnd(Entity entity,float Value)
	{
		entity.OnGetInk -= LessInkGet;
	}
	public override void OnAdd(Entity entity,float Value)
	{
		
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}
	public override void OnTriger(Entity entity, float Value)
	{
		entity.ChangeInkAmount(Value);
	}
	void LessInkGet(Entity self,InkData inkData)
	{
		inkData.inkAmount *= 0.8f;
	}
}
