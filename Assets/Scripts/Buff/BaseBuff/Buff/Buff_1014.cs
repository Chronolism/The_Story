public class Buff_1014 : BuffBase
{

    public override void OnStart(Entity entity, float Value)
    {
        entity.OnAddServitor += OnAddServitor;
        entity.OnRemoveServitor += OnRemoveServitor;
        
    }

    public override void OnEnd(Entity entity, float Value)
    {
        entity.OnAddServitor -= OnAddServitor;
        entity.OnRemoveServitor -= OnRemoveServitor;

    }

    public override void OnAdd(Entity entity, float Value)
    {
        foreach (Entity entity1 in entity.Servitors)
        {
            entity1.maxSpeed += Value / 100;
        }
    }
    public override void OnRemove(Entity entity, float Value)
    {
        foreach (Entity entity1 in entity.Servitors)
        {
            entity1.maxSpeed -= Value / 100;
        }
    }

    private void OnAddServitor(Entity arg0, Entity arg1)
    {
        arg1.maxSpeed += Amount / 100;
    }
    private void OnRemoveServitor(Entity arg0, Entity arg1)
    {
        arg1.maxSpeed -= Amount / 100;
    }
}
