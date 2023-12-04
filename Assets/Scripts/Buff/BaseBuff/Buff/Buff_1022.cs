using UnityEngine.Events;

public class Buff_1022 : BuffBase
{
	//ÉÆË®Buff
	bool m_ifBuffWorking = false;
	public bool BuffWorking
	{
		get { return m_ifBuffWorking; }
		set
		{
			if (m_ifBuffWorking != value)
			{
				m_ifBuffWorking = value;
				if (m_ifBuffWorking)
				{
					entity.maxSpeed_Pre += Amount / 100;
				}
				else
				{
					entity.maxSpeed_Pre -= Amount / 100;
				}
			}
		}
	}
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnAddBuff += _CouplingOnAddBuff;
		entity.OnRemoveBuff += _CouplingOnRemoveBuff;
		foreach (var buff in entity.skill.buffList)
        {
			if (buff is Buff_4001 b) { BuffWorking = true ; b.trigerBuff = false; }
		}
	}
	public override void OnEnd(Entity entity,float Value)
	{
		entity.OnAddBuff -= _CouplingOnAddBuff;
		entity.OnRemoveBuff -= _CouplingOnRemoveBuff;
		foreach (var buff in entity.skill.buffList)
		{
			if (buff is Buff_4001 b) { BuffWorking = false; b.trigerBuff =true; }
		}
		BuffWorking = false;
	}
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
		
	}
	private void _CouplingOnAddBuff(Entity self, BuffBase buff, float value)
    {
		if (buff is Buff_4001 b) { BuffWorking = true; b.trigerBuff = false; }
	}
	private void _CouplingOnRemoveBuff(Entity self, BuffBase buff, float value)
	{
		if (buff is Buff_4001 b) { BuffWorking = false; b.trigerBuff = true; } 
	}
}
