public class Buff_1018 : BuffBase
{
    public override void OnStart(Entity entity, float Value)
    {
        entity.AfterGetInk += WhenAddInkAddEnergy;

    }
    public override void OnEnd(Entity entity, float Value)
    {
        entity.AfterGetInk -= WhenAddInkAddEnergy;
    }
    public override void OnAdd(Entity entity, float Value)
    {
    }
    public override void OnRemove(Entity entity, float Value)
    {
    }
    void WhenAddInkAddEnergy(Entity self, InkData inkData)
    {
        self.AddEnergy(new InkData(0,inkData.inkAmount*Amount/100,true));
    }
}
