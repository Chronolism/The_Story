public class Buff_5009 : BuffBase,IUpdataBuff
{
	//魔法树丛
	private bool m_Hide = false;
	private bool m_trigerBuff = false;

	public bool trigerHide
	{
		get { return m_Hide; }
		set
		{
			if (m_Hide != value)
			{
				m_Hide = value;
				Mono_QAReportText.Instance?.Report(m_Hide ? "魔法树丛：隐匿" : "魔法树丛：隐匿失效");
				if (m_Hide)
				{
					entity.HideEntityClient(true);
				}
				else
				{
					entity.HideEntityClient(false);
				}
			}
		}
	}
	public bool trigerBuff
	{
		get { return m_trigerBuff; }
		set
		{
			if (m_trigerBuff != value)
			{
				m_trigerBuff = value;
				Mono_QAReportText.Instance?.Report(m_trigerBuff ? "魔法树丛：隐匿减速并回复" : "魔法树丛：隐匿速度还原回复取消");
				if (m_trigerBuff)
				{
					entity.maxSpeed_Pre -= Amount / 100; ;
				}
				else
				{
					entity.maxSpeed_Pre += Amount / 100; ;
				}
			}
		}
	}

	public override void OnStart(Entity entity, float Value)
	{
		trigerBuff = true;
	}
	public override void OnAddEffect(Entity entity, float Value)
	{
		if (entity != DataMgr.Instance.activePlayer)
		{
			trigerHide = true;
		}
	}

	public override void OnRemoveEffect(Entity entity, float Value)
	{
		if (entity != DataMgr.Instance.activePlayer)
		{
			trigerHide = false;
		}
	}
	public override void OnEnd(Entity entity, float Value)
	{
		trigerBuff = false;
	}
	public override void OnAdd(Entity entity, float Value)
	{

	}
	public override void OnRemove(Entity entity, float Value)
	{
	}

	public void Updata(Entity entity)
	{
		if (m_trigerBuff)
		{
			entity.AddEnergy(new InkData(0, UnityEngine.Time.fixedDeltaTime * 5, true));
		}
	}
}
