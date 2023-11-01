public class Buff_1017 : BuffBase
{
	public override void OnStart(Entity entity,float Value)
	{
		entity.AfterGetInk += WhenAddInkAddBlood;

    }
	public override void OnEnd(Entity entity,float Value)
	{
        entity.AfterGetInk -= WhenAddInkAddBlood;
    }
	public override void OnAdd(Entity entity,float Value)
	{
	}
	public override void OnRemove(Entity entity,float Value)
	{
	}
	void WhenAddInkAddBlood(Entity self , InkData inkData)
	{
		self.ChangeBlood(self, new ATKData(1, 1, inkData.inkAmount * Amount / 100, 0, 1, AtkType.cure));
	}
}
