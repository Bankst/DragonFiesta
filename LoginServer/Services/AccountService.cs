using DFEngine.Database;

namespace LoginServer.Services
{
	internal class AccountService
	{
		internal static void Login(string userName, string password, out int userNo, out bool blocked, out bool canLogin)
		{
			using (var usp_User_LoginGame = new StoredProcedure("usp_User_loginGame", ServerMain.AccountDb))
			{
				usp_User_LoginGame.AddParameter("userID", userName, 20);
				usp_User_LoginGame.AddParameter("userPW", password, 32);

				usp_User_LoginGame.AddOutput<int>("userNo");
				usp_User_LoginGame.AddOutput<byte>("authID");
				usp_User_LoginGame.AddOutput<int>("block");
				usp_User_LoginGame.AddOutput<int>("isLoginable");

				usp_User_LoginGame.Run();

				userNo = usp_User_LoginGame.GetOutput<int>("userNo");
				blocked = usp_User_LoginGame.GetOutput<int>("block") == 1;
				canLogin = usp_User_LoginGame.GetOutput<int>("isLoginable") == 1;
			}
		}
	}
}
