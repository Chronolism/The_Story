using System;

public class Buff_2003 : BuffBase
{
    public override void OnStart(Entity entity, float Value)
    {
        entity.OnAddBuff += CanGiddy;
    }

    public override void OnEnd(Entity entity, float Value)
    {
        entity.OnAddBuff -= CanGiddy;
    }
    public override void OnAdd(Entity entity,float Value)
	{
        foreach (var buff in entity.skill.buffList.FindAll(i => i.buffData.id == 3010))
        {
            (buff as Buff_3010).giddyTime += Value/100;
        }
    }
	public override void OnRemove(Entity entity,float Value)
	{
        foreach (var buff in entity.skill.buffList.FindAll(i => i.buffData.id == 3010))
        {
            (buff as Buff_3010).giddyTime -= Value/100;
        }
    }
    private void CanGiddy(Entity self, BuffBase buff, float value)
    {
        if(buff is Buff_3010 buff_3010)
            buff_3010.giddyTime = Amount/100;
    }
}
