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
		Register(1024, typeof(Buff_1024));
		Register(1025, typeof(Buff_1025));
		Register(1026, typeof(Buff_1026));
		Register(1027, typeof(Buff_1027));
		Register(2001, typeof(Buff_2001));
		Register(2002, typeof(Buff_2002));
		Register(2003, typeof(Buff_2003));
		Register(3001, typeof(Buff_3001));
		Register(3002, typeof(Buff_3002));
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
