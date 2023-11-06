using System.Collections.Generic;
using UnityEngine;

public class Buff_3008 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
	}
	public override void OnEnd(Entity entity,float Value)
	{
	}
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}

	public override void OnTriger(Entity entity, float Value)
	{
		int amount;
		Stack<Servitor> stack = new Stack<Servitor>();
		foreach(Servitor servitor in entity.Servitors)
		{
            stack.Push(servitor);

        }
		amount = stack.Count;
		Servitor ser;
        while (stack.Count > 0)
		{
			ser= stack.Pop();
            entity.RemoveServitor(ser);
			ser.EntityDie();
        }
		if (amount <= 1)
		{
			entity.AddBuff(2005, 50, 6, entity);
		}
		else if (amount == 2) 
		{
            entity.AddBuff(2005, 100, 8, entity);
        }
		else
		{
            entity.AddBuff(2005, 150, 9, entity);
        }
	}
}
