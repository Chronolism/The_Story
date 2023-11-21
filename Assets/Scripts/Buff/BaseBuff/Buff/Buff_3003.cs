using System;

public class Buff_3003 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		entity.AfterGetInk += TrigerEffect;
	}

	public override void OnEnd(Entity entity,float Value)
	{
        entity.AfterGetInk -= TrigerEffect;
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
    private void TrigerEffect(Entity arg0, InkData arg1)
    {
		if (arg1.inkAmount > 0)
		{
            arg0.GetComponent<EntityEffect>().ShowEffect(3003);
        }
    }
}
