public class Buff_1002 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{

	}
	public override void OnEnd(Entity entity,float Value)
	{

	}
    public override void OnAdd(Entity entity, float Value)
    {
        entity.maxBlood_Pre += Value/100;
    }
    public override void OnRemove(Entity entity, float Value)
    {
        entity.maxBlood_Pre -= Value/100;
    }
}
