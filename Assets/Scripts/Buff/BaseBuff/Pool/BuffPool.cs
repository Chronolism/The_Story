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
		Register(2001, typeof(Buff_2001));
		Register(2002, typeof(Buff_2002));
		Register(3001, typeof(Buff_3001));
		Register(3002, typeof(Buff_3002));
		Register(3003, typeof(Buff_3003));
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
