public class Buff_1021 : BuffBase
{
	//深水通行
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
					entity.ChangeMapCollider(MapColliderType.Water, true);
				}
				else
				{
					entity.ChangeMapCollider(MapColliderType.Water, false);
				}
			}
		}
	}
	public override void OnStart(Entity entity, float Value)
	{
		BuffWorking = true;
		entity.OnAddBuff += _CouplingOnAddBuff;
		entity.OnRemoveBuff += _CouplingOnRemoveBuff;
		foreach (var buff in entity.skill.buffList)
		{
			if (buff is Buff_5001 b) { BuffWorking = true; b.trigerBuff = false; b.trigerWet = false; }
		}
	}
	public override void OnEnd(Entity entity, float Value)
	{
		entity.OnAddBuff -= _CouplingOnAddBuff;
		entity.OnRemoveBuff -= _CouplingOnRemoveBuff;
		BuffWorking = false;
	}
	public override void OnAdd(Entity entity, float Value)
	{
	}
	public override void OnRemove(Entity entity, float Value)
	{
	}
	private void _CouplingOnAddBuff(Entity self, BuffBase buff, float value)
	{
		if (buff is Buff_5001 b) { BuffWorking = true; b.trigerBuff = false ; b.trigerWet = false; }
	}
	private void _CouplingOnRemoveBuff(Entity self, BuffBase buff, float value)
	{
		if (buff is Buff_5001 b) { BuffWorking = false; b.trigerBuff = true ; b.trigerWet = true; }
	}
}
