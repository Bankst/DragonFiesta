using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Database;

namespace LoginServer.Services
{
	internal class AccountService
	{
		internal static void Login(string userName, string password, out int userNo, out bool blocked, out bool canLogin)
		{
			using (var spo = new StoredProcedure("usp_User_loginGame", DB.GetDatabaseClient(DatabaseType.Account).mConnection))
			{
				spo.AddParameter("userID", userName, 20);
				spo.AddParameter("userPW", password, 32);

				spo.AddOutput<int>("userNo");
				spo.AddOutput<byte>("authID");
				spo.AddOutput<int>("block");
				spo.AddOutput<int>("isLoginable");

				spo.Run();

				userNo = spo.GetOutput<int>("userNo");
				blocked = spo.GetOutput<int>("block") == 1;
				canLogin = spo.GetOutput<int>("isLoginable") == 1;
			}
		}
	}
}
