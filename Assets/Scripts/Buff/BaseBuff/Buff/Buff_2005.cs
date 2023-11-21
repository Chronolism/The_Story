using UnityEngine;
public class Buff_2005 : BuffBase
{
	public bool canRewrite;
	public override void OnStart(Entity entity,float Value)
	{
		entity.OnGetInk += CanNotUseInk;
		entity.OnGetEnergy += CanNotGetEnergy;
        entity.OnTouchEntity += HurtServitor;
        //体型变大
        entity.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
		if (entity.inkAmount <= 0) entity.inkAmount += 1;
    }
	public override void OnEnd(Entity entity,float Value)
	{
        entity.OnGetInk -= CanNotUseInk;
        entity.OnGetEnergy -= CanNotGetEnergy;
        entity.OnTouchEntity += HurtServitor;
        //体型变小
        entity.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
    }
	public override void OnAdd(Entity entity,float Value)
	{
		entity.maxSpeed += Value / 100;
	}
	public override void OnRemove(Entity entity,float Value)
	{
        entity.maxSpeed -= Value / 100;
    }

	void CanNotUseInk(Entity self,InkData inkData)
	{
		if (canRewrite && inkData.inkAmount < 0)
		{
			inkData.ifTurn = false;
		}
	}
	void CanNotGetEnergy(Entity self , InkData energy)
	{
		energy.ifTurn = false;
	}

    private void HurtServitor(Entity slef, Entity entity, ATKData aTKData)
    {
        if (!aTKData.canAtk && entity is Player)
        {
            aTKData.canAtk = true;
            slef.AtkEntity(entity, aTKData);
        }
    }
}
