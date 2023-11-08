

using UnityEngine;

public class Buff_1003 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		

    }

	public override void OnEnd(Entity entity,float Value)
	{
        
    }
    public override void OnAdd(Entity entity, float Value)
    {
        Debug.Log("º”ÀŸ");
        entity.maxSpeed += Value/100;
    }
    public override void OnRemove(Entity entity, float Value)
    {
        entity.maxSpeed -= Value/100;
    }
}
