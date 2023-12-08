public class Buff_5001 : BuffBase,IUpdataBuff
{
	//深水区
	private bool m_trigerBuff = true;
	private bool m_wet = true;
	public bool trigerBuff
	{
		get { return m_trigerBuff; }
		set
		{
			if (m_trigerBuff != value)
			{
				m_trigerBuff = value;
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
		trigerWet = true;
	}
	public override void OnEnd(Entity entity, float Value)
	{
		trigerWet = false;
	}
	public override void OnAdd(Entity entity, float Value)
	{

	}
	public override void OnRemove(Entity entity, float Value)
	{
	}

	public void Updata(Entity entity)
	{
		if (trigerBuff)
		{
			trigerBuff = false;
			entity.DropMap();
		}
	}
}
