public class Buff_1016 : BuffBase,IUpdataBuff
{
	bool ifHaveRewrite;
	public override void OnStart(Entity entity,float Value)
	{
	}
	public override void OnEnd(Entity entity,float Value)
	{
	}
	public override void OnAdd(Entity entity,float Value)
	{
		if (ifHaveRewrite)
		{
            entity.AddBuff(1014, Value, entity);
        }
	}
	public override void OnRemove(Entity entity,float Value)
	{
        if (ifHaveRewrite)
        {
            entity.RemoveBuff(1014, Value, entity);
        }
    }

	public void Updata(Entity entity)
	{
		if (ifHaveRewrite)
		{
			if (!entity.canRewrite)
			{
				ifHaveRewrite = false;
                entity.RemoveBuff(1014, Amount, entity);
            }
		}
		else
		{
			if (entity.canRewrite)
			{
				ifHaveRewrite = true;
				entity.AddBuff(1014, Amount, entity);
            }
		}
		
	}
}
