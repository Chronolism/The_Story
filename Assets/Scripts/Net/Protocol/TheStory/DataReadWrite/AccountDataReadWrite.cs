using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
namespace TheStory
{
	public class AccountData
	{
		public string account;
		public string password;
	}

	public static class AccountDataReadWrite
	{
		public static void WriteAccountData(this NetworkWriter writer, AccountData value)
		{
			writer.Write(value.account);
			writer.Write(value.password);
		}
		public static AccountData ReadAccountData(this NetworkReader reader)
		{
			AccountData value = new AccountData();
			value.account = reader.ReadString();
			value.password = reader.ReadString();
			return value;
		}
	}
}