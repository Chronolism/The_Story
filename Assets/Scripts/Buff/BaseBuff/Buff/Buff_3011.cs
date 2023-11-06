using System.Collections.Generic;

public class Buff_3011 : BuffBase
{
	public BuffBase mainSkill;
    public List<ForeShadowAttack> shadowAttacks;

	public void Init(string buffId, float amount, Entity buffOwn, BuffBase mainSkill , List<ForeShadowAttack> foreShadowAttacks)
	{
		base.Init(buffId, amount, buffOwn);
		this.mainSkill = mainSkill;
		shadowAttacks = foreShadowAttacks;
	}

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
		base.OnTriger(entity, Value);
		foreach(ForeShadowAttack attack in shadowAttacks.ToArray())
		{
			attack.StopAttack();
		}
        entity.skill.buffList[0] = mainSkill;
        entity.AddEnergy(new InkData(0, 99999, true));

	}
}
