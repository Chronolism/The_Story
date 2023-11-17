public class Buff_1015 : BuffBase
{
	float buffTime;
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnUseProp += AddCannotHurt;
		entity.OnUseSkill += AddCannotHurt;

    }
	public override void OnEnd(Entity entity,float Value)
	{
        entity.OnUseProp -= AddCannotHurt;
        entity.OnUseSkill -= AddCannotHurt;
    }
	public override void OnAdd(Entity entity,float Value)
	{
        buffTime = Value;
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}

	void AddCannotHurt(Entity self,PropData prop)
	{
		self.AddBuff(1012, 1, buffTime, self);
	}
    void AddCannotHurt(Entity self, BuffBase buff)
    {
        self.AddBuff(1012, 1, buffTime, self);
    }
}
