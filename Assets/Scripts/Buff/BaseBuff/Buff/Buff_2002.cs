public class Buff_2002 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnAddBuff += CanreWrite;

    }
	public override void OnEnd(Entity entity,float Value)
	{
		entity.OnAddBuff -= CanreWrite;
    }
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}
	void CanreWrite(Entity self, BuffBase buff, float value)
	{
        if (buff is Buff_2005 buff_2005)
        {
            buff_2005.canRewrite = true;
        }
    }
}
