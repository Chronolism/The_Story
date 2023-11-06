public class Buff_1012 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnAtked += CannotHurt;

    }
	public override void OnEnd(Entity entity,float Value)
	{
		entity.OnAtked -= CannotHurt;
	}
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}

	public void CannotHurt(Entity self, Entity target, ATKData atk)
	{
		atk.canAtk = false;
	}
}
