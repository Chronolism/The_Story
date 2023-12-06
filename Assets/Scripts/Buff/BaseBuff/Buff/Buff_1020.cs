public class Buff_1020 : BuffBase
{
	//浅水通行
	bool m_ifBuffWorking = false;
	public bool BuffWorking
	{
		get { return m_ifBuffWorking; }
		set
		{
			if (m_ifBuffWorking != value)
			{
				m_ifBuffWorking = value;
			}
		}
	}
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnAddBuff += _CouplingOnAddBuff;
		entity.OnRemoveBuff += _CouplingOnRemoveBuff;
		foreach (var buff in entity.skill.buffList)
		{
			if (buff is Buff_4001 b) { 
				BuffWorking = true; 
				b.trigerBuff = false; }
			if (buff is Buff_5001 b2) { 
				BuffWorking = true; 
				b2.trigerBuff = false; }
		}
		Mono_QAReportText.Instance?.Report("获得浅水通行");
	}
	public override void OnEnd(Entity entity,float Value)
	{
		entity.OnAddBuff -= _CouplingOnAddBuff;
		entity.OnRemoveBuff -= _CouplingOnRemoveBuff;
		BuffWorking = false;
		Mono_QAReportText.Instance?.Report("失去浅水通行");
	}
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}
	private void _CouplingOnAddBuff(Entity self, BuffBase buff, float value)
	{
		if (buff is Buff_4001 b) { 
			BuffWorking = true; 
			b.trigerBuff = false; }
		if (buff is Buff_5001 b2) { 
			BuffWorking = true; 
			b2.trigerBuff = false; }
	}
	private void _CouplingOnRemoveBuff(Entity self, BuffBase buff, float value)
	{
		if (buff is Buff_4001 b) { 
			BuffWorking = false; 
			b.trigerBuff = true; }
		if (buff is Buff_5001 b2) { 
			BuffWorking = false; 
			b2.trigerBuff = true; }
	}
}
