
using UnityEngine;

public class Buff_4001 : BuffBase
{
	private bool m_trigerBuff = true;
	public bool trigerBuff
	{
		get { return m_trigerBuff; }
		set 
		{
			if(m_trigerBuff != value) 
			{
				m_trigerBuff = value;
				ChangeEntityState(m_trigerBuff);

            }
		}
	}
	
	public override void OnStart(Entity entity,float Value)
	{
        ChangeEntityState(m_trigerBuff);
    }
	public override void OnEnd(Entity entity,float Value)
	{
		if (m_trigerBuff)
		{
            ChangeEntityState(false, Value);
        }
    }
	public override void OnAdd(Entity entity,float Value)
	{
        
    }
	public override void OnRemove(Entity entity,float Value)
	{
	}

	void ChangeEntityState(bool add , float value = 0)
	{

		if (add)
		{
			Mono_QAReportText.Instance?.Report("浅水区：减速");
			entity.maxSpeed_Pre -= Amount/100;
		}
		else
		{
			Mono_QAReportText.Instance?.Report("浅水区：速度还原");
			entity.maxSpeed_Pre += Amount/100;
		}
	}

}
