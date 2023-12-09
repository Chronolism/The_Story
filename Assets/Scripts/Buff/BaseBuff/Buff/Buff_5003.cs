public class Buff_5003 : BuffBase,IUpdataBuff
{
	//��ˮ��
	private bool m_trigerBuff = true;
	private bool m_wet = false;
	private float m_energyRecovery = 5f;
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
			Mono_QAReportText.Instance?.Report(m_trigerBuff ? "ħ��ˮ�򣺻�����Ч" : "ħ��ˮ�򣺻�����Ч");
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
					Mono_QAReportText.Instance?.Report("ħ��ˮ�򣺼���");
					entity.maxSpeed_Pre -= Amount / 100;
				}
				else
				{
					Mono_QAReportText.Instance?.Report("ħ��ˮ���ٶȻ�ԭ");
					entity.maxSpeed_Pre += Amount / 100;
				}
			}
		}
	}
	public bool trigerEnergyRecovery
	{
		get { return m_energyRecovery != 5f; }
		set
		{
			if ((m_energyRecovery != 5f) != value)
			{
				m_energyRecovery = value ? 5f:10f ;
				Mono_QAReportText.Instance?.Report(value ? "ħ��ˮ��������" : "ħ��ˮ��ǿ����");
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

    public void Updata(Entity entity)
    {
        if (m_trigerBuff)
        {
			entity.AddEnergy(new InkData(0, UnityEngine.Time.fixedDeltaTime * m_energyRecovery, true));
        }
    }
}
