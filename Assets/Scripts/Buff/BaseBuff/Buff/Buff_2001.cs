using System;

public class Buff_2001 : BuffBase
{
    public float addSpeed;

	public override void OnStart(Entity entity,float Value)
	{
		entity.OnAddServitor += OnAddServitor;
		entity.OnRemoveServitor += OnRemoveServitor;
        addSpeed = Value;

    }

	public override void OnEnd(Entity entity,float Value)
	{
        entity.OnAddServitor -= OnAddServitor;
        entity.OnRemoveServitor -= OnRemoveServitor;
    }


    private void OnAddServitor(Entity arg0, Entity arg1)
    {
        arg1.maxSpeed += addSpeed/100;
    }
    private void OnRemoveServitor(Entity arg0, Entity arg1)
    {
        arg1.maxSpeed -= addSpeed/100;
    }
}
