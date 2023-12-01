using System.Collections.Generic;
using UnityEngine;

public class Buff_3010 : BuffBase,Observer<ForeShadowAttack>
{
	public bool ifRecovery;
	public float giddyTime;
	int amount;
	public List<ForeShadowAttack> shadowAttacks = new List<ForeShadowAttack>();
	public Entity trigerEntity;
    public override void OnStart(Entity entity,float Value)
	{
	}
	public override void OnEnd(Entity entity,float Value)
	{
	}
	public override void OnAdd(Entity entity,float Value)
	{
		amount = (int)Value;
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}
	public override void OnTriger(Entity entity, float Value)
	{
		if (ifRecovery)
		{
            ForeShadowAttack foreShadowAttack = EntityFactory.Instance.CreatAttack(entity, 11002, entity.transform.position, Vector3.zero, new List<float>() { giddyTime }) as ForeShadowAttack;
            shadowAttacks.Add(foreShadowAttack);
            foreShadowAttack.Observers.Add(this);


            this.trigerEntity = entity;
            Buff_3011 buff_3011 = DataMgr.Instance.GetBuff(3011) as Buff_3011;
			buff_3011.Init(entity,"3011" + entity.netId, Value, entity , entity.skill.buffList[0] , shadowAttacks);

            entity.skill.buffList[0] = buff_3011;
		}
		else
		{
			if (shadowAttacks.Count >= amount) shadowAttacks[0].StopAttack();
			ForeShadowAttack foreShadowAttack = EntityFactory.Instance.CreatAttack(entity, 11002, entity.transform.position, Vector3.zero, new List<float>() { giddyTime }) as ForeShadowAttack;
            shadowAttacks.Add(foreShadowAttack);
            foreShadowAttack.Observers.Add(this);

		}
	}

	public void ToUpdate(ForeShadowAttack value)
	{
        shadowAttacks.Remove(value);
		if(shadowAttacks.Count == 0)
		{
			if(ifRecovery && trigerEntity.MainSkill is Buff_3011 buff_3011)
			{
				trigerEntity.skill.buffList[0] = buff_3011.mainSkill;
			}
		}
    }
}
