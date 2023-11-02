using UnityEngine;

public class Buff_3007 : BuffBase
{
	public bool ifTurn;
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
		Debug.Log(ifTurn);
        ArrowAttack arrowAttack = EntityFactory.Instance.CreatAttack<ArrowAttack>(entity, 11001, entity.dir == 0 ? Vector3.right : entity.dir == 1 ? Vector3.up : entity.dir == 2 ? Vector3.left : Vector3.down);
		arrowAttack.ifTurn = ifTurn;

    }
}
