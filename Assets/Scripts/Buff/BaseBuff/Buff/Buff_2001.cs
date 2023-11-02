using System;
using UnityEngine;

public class Buff_2001 : BuffBase
{

	public override void OnStart(Entity entity,float Value)
	{
        entity.OnAddBuff += ArrowCanTurn;
        foreach(var buff in entity.FindBuffs(3007))
        {
            (buff as Buff_3007).ifTurn = true;
        }
    }

	public override void OnEnd(Entity entity,float Value)
	{
        entity.OnAddBuff -= ArrowCanTurn;
        foreach (var buff in entity.FindBuffs(3007))
        {
            (buff as Buff_3007).ifTurn = false;
        }
    }


    private void OnAddServitor(Entity arg0, Entity arg1)
    {
        
    }
    private void OnRemoveServitor(Entity arg0, Entity arg1)
    {
        
    }

    void ArrowCanTurn(Entity self ,BuffBase buff , float value)
    {
        if(buff is Buff_3007 buff_3007)
        {
            buff_3007.ifTurn = true;
        }
    }
}
