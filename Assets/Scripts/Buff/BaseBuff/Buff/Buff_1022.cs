using UnityEngine.Events;

public class Buff_1022 : BuffBase
{
	//善水Buff
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
					Mono_QAReportText.Instance?.Report("善水：加速");
				}
				else
				{
					entity.maxSpeed_Pre -= Amount / 100;
					Mono_QAReportText.Instance?.Report("善水：速度还原");
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
			if (buff is Buff_5001 b2) { BuffWorking = true; b2.trigerBuff = false; b2.trigerWet = false; }
		}
	}
	public override void OnEnd(Entity entity,float Value)
	{
		entity.OnAddBuff -= _CouplingOnAddBuff;
		entity.OnRemoveBuff -= _CouplingOnRemoveBuff;
		foreach (var buff in entity.skill.buffList)
		{
			if (buff is Buff_4001 b) { BuffWorking = false; b.trigerBuff =true; }
			if (buff is Buff_5001 b2) { BuffWorking = false; b2.trigerBuff = true; b2.trigerWet = true; }
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
		if (buff is Buff_5001 b2) { BuffWorking = true; b2.trigerBuff = false;b2.trigerWet = false; }
	}
	private void _CouplingOnRemoveBuff(Entity self, BuffBase buff, float value)
	{
		if (buff is Buff_4001 b) { BuffWorking = false;  }
		if (buff is Buff_5001 b2) { BuffWorking = false; }
	}
}
