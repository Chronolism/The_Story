public class Buff_1013 : BuffBase
{
    public override void OnStart(Entity entity, float Value)
    {
        entity.OnTurn += OnTurn;

    }

    public override void OnEnd(Entity entity, float Value)
    {
        entity.OnTurn -= OnTurn;
    }
    public override void OnAdd(Entity entity, float Value)
    {
    }
    public override void OnRemove(Entity entity, float Value)
    {
    }
    private void OnTurn(Entity arg0, Entity arg1, InkData arg2)
    {
        arg2.ifTurn = false;
    }
}
