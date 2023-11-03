public class Buff_2004 : BuffBase
{
    public override void OnStart(Entity entity, float Value)
    {
        entity.OnAddBuff += CanRecovery;
        foreach (var buff in entity.skill.buffList.FindAll(i => i.buffData.id == 3010))
        {
            (buff as Buff_3010).ifRecovery = true;
        }
    }

    public override void OnEnd(Entity entity, float Value)
    {
        entity.OnAddBuff -= CanRecovery;
        foreach (var buff in entity.skill.buffList.FindAll(i => i.buffData.id == 3010))
        {
            (buff as Buff_3010).ifRecovery = false;
        }
    }
    public override void OnAdd(Entity entity, float Value)
    {

    }
    public override void OnRemove(Entity entity, float Value)
    {

    }
    private void CanRecovery(Entity self, BuffBase buff, float value)
    {
        if (buff is Buff_3010 buff_3010)
            buff_3010.ifRecovery = true;
    }
}
