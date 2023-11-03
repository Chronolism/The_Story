using System.Collections.Generic;
using UnityEngine;

public class Buff_3006 : BuffBase
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
		EntityFactory.Instance.CreatAttack(entity, 11003, entity.transform.position, Vector3.zero, new List<float>() { Value });
	}
}
