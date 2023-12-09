public class Buff_5001 : BuffBase
{
	//深水区
	private bool m_trigerBuff = true;
	private bool m_wet = false;
	public bool trigerBuff
	{
		get { return m_trigerBuff; }
		set
		{
			if (m_trigerBuff != value)
			{
				m_trigerBuff = value;
				ChangeEntityState(m_trigerBuff);
			}
		}
	}
	public bool trigerWet
	{
		get { return m_wet; }
		set
		{
			if (m_wet != value)
			{
				m_wet = value;
				if (m_wet)
				{
					Mono_QAReportText.Instance?.Report("深水区：减速");
					entity.maxSpeed_Pre -= Amount / 100;
				}
				else
				{
					Mono_QAReportText.Instance?.Report("深水区：速度还原");
					entity.maxSpeed_Pre += Amount / 100;
				}
			}
		}
	}

	public override void OnStart(Entity entity, float Value)
	{
		ChangeEntityState(m_trigerBuff);
		trigerWet = true;
	}
	public override void OnEnd(Entity entity, float Value)
	{
		if (m_trigerBuff)
		{
			ChangeEntityState(false, Value);
		}
		trigerWet = false;
	}
	public override void OnAdd(Entity entity, float Value)
	{

	}
	public override void OnRemove(Entity entity, float Value)
	{
	}

	void ChangeEntityState(bool add, float value = 0)
	{
		if (add)
		{
			
		}
		else
		{
			
		}
	}
}
