using System;

public class Buff_1013 : BuffBase
{
    public override void OnStart(Entity entity, float Value)
    {
        entity.OnTurn += OnTurn;
        entity.OnTouchEntity += HurtEntity;
    }
    public override void OnEnd(Entity entity, float Value)
    {
        entity.OnTurn -= OnTurn;
        entity.OnTouchEntity -= HurtEntity;
    }
    public override void OnAdd(Entity entity, float Value)
    {
        entity.maxSpeed_Pre += Value / 100;
    }
    public override void OnRemove(Entity entity, float Value)
    {
        entity.maxSpeed_Pre -= Value / 100;
    }
    private void OnTurn(Entity arg0, Entity arg1, InkData arg2)
    {
        arg2.ifTurn = false;
    }
    public override void OnAddEffect(Entity entity, float Value)
    {
        entity.GetComponent<EntityEffect>().ShowEffectOnClient(3004);
    }

    public override void OnRemoveEffect(Entity entity, float Value)
    {
        entity.GetComponent<EntityEffect>().HideEffectOnClient(3004);
    }
    private void HurtEntity(Entity slef, Entity entity, ATKData aTKData)
    {
        if (!aTKData.canAtk)
        {
            aTKData.canAtk = true;
            slef.AtkEntity(entity,aTKData);
        }
    }
}
