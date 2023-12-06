public class Buff_5001 : BuffBase,IUpdataBuff
{
	//ÉîË®Çø
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
					entity.maxSpeed_Pre -= Amount / 100;
				}
				else
				{
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
