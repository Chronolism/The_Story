using System;
using System.Collections.Generic;
public class BuffPool
{
	private Dictionary<int, Type> buffs = new Dictionary<int, Type>();
	public BuffPool()
	{
		Register(1001, typeof(Buff_1001));
		Register(1002, typeof(Buff_1002));
		Register(1003, typeof(Buff_1003));
		Register(1004, typeof(Buff_1004));
		Register(1005, typeof(Buff_1005));
		Register(1006, typeof(Buff_1006));
		Register(1007, typeof(Buff_1007));
		Register(1008, typeof(Buff_1008));
		Register(1009, typeof(Buff_1009));
		Register(1010, typeof(Buff_1010));
		Register(1011, typeof(Buff_1011));
		Register(1012, typeof(Buff_1012));
		Register(1013, typeof(Buff_1013));
		Register(1014, typeof(Buff_1014));
		Register(1015, typeof(Buff_1015));
		Register(1016, typeof(Buff_1016));
		Register(1017, typeof(Buff_1017));
		Register(1018, typeof(Buff_1018));
		Register(1019, typeof(Buff_1019));
		Register(1020, typeof(Buff_1020));
		Register(1021, typeof(Buff_1021));
		Register(1022, typeof(Buff_1022));
		Register(1023, typeof(Buff_1023));
		Register(2001, typeof(Buff_2001));
		Register(2002, typeof(Buff_2002));
		Register(2003, typeof(Buff_2003));
		Register(2004, typeof(Buff_2004));
		Register(2005, typeof(Buff_2005));
		Register(3001, typeof(Buff_3001));
		Register(3002, typeof(Buff_3002));
		Register(3003, typeof(Buff_3003));
		Register(3004, typeof(Buff_3004));
		Register(3005, typeof(Buff_3005));
		Register(3006, typeof(Buff_3006));
		Register(3007, typeof(Buff_3007));
		Register(3008, typeof(Buff_3008));
		Register(3009, typeof(Buff_3009));
		Register(3010, typeof(Buff_3010));
		Register(3011, typeof(Buff_3011));
		Register(3012, typeof(Buff_3012));
		Register(3013, typeof(Buff_3013));
		Register(4001, typeof(Buff_4001));
		Register(5001, typeof(Buff_5001));
		Register(5002, typeof(Buff_5002));
		Register(5003, typeof(Buff_5003));
		Register(5004, typeof(Buff_5004));
		Register(5005, typeof(Buff_5005));
		Register(5006, typeof(Buff_5006));
		Register(5007, typeof(Buff_5007));
		Register(5008, typeof(Buff_5008));
		Register(5009, typeof(Buff_5009));
		Register(5010, typeof(Buff_5010));
		Register(5011, typeof(Buff_5011));
	}
	private void Register(int id, Type buffType)
	{
		buffs.Add(id, buffType);
	}
	public BuffBase GetBuff(int id)
	{
		if (!buffs.ContainsKey(id))
			return null;
		return Activator.CreateInstance(buffs[id]) as BuffBase;
	}
}
