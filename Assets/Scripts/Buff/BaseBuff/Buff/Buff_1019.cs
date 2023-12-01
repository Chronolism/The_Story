public class Buff_1019 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		entity.ChangeMapCollider(MapColliderType.Gully, true);
	}
	public override void OnEnd(Entity entity,float Value)
	{
        entity.ChangeMapCollider(MapColliderType.Gully, false);
    }
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}
}
