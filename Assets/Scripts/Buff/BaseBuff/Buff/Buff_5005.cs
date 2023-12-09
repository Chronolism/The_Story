public class Buff_5005 : BuffBase
{
	//Ê÷´Ô
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
				Mono_QAReportText.Instance?.Report(m_Hide ? "Ê÷´Ô£ºÒþÄä" : "Ê÷´Ô£ºÒþÄäÊ§Ð§");
				if(m_Hide)
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
				Mono_QAReportText.Instance?.Report(m_trigerBuff ? "Ê÷´Ô£ºÒþÄä¼õËÙ" : "Ê÷´Ô£ºÒþÄäËÙ¶È»¹Ô­");
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
        if(entity != DataMgr.Instance.activePlayer)
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

}
