using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Game.Accounts;
using DragonFiesta.Login.Network.InternHandler.Server;
using System;
using System.Text.RegularExpressions;

namespace DragonFiesta.Login.ServerConsole
{
    [ConsoleCommandCategory("Account")]
    public sealed class CONSOLE_ACCOUNT_HANDLER
    {
        [ConsoleCommand("ban")]
        public static bool CMD_ACCOUNT_BAN(string[] Params)
        {
            var banState = false;
            long banTime = 0;
            if (Params.Length == 0 || Params.Length >= 2 && StringExtensions.ParseBool(Params[1], out banState) & !long.TryParse(Params[1], out banTime))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid input Use Account ban <Accountname> <bool> or <BanTime> <Reason>");
                return true;
            }

            if (!AccountManager.GetAccountByName(Params[0], out var pAccount))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account {0} Not found!", Params[0]);
                return true;
            }
            //GetReason
	        var reason = Params.Length == 2 ? "Banned by Console No Reason" : String.Join(" ", 3, Params.Length);

            if (banTime > 1)
            {
                pAccount.BanTime = banTime;
                pAccount.BanDate = DateTime.Now;
                pAccount.BanReason = reason;
                CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Ban Account {0} Success!!", pAccount.Name);
            }
            else
            {
                if (banState)
                {
                    pAccount.BanTime = int.MaxValue;// yeahr its better unban in 31k Years :D
                    pAccount.BanDate = DateTime.Now;
                    pAccount.BanReason = reason;
                }
                else
                {
                    pAccount.BanDate = DateTime.Now;
                    pAccount.BanTime = 0;
                    CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Unban Account {0} Success!!", pAccount.Name);
                }
            }

            if (pAccount.IsOnline)
                ServerAccountMethods.SendAccountUpdate(pAccount);
            else
                AccountManager.UpdateAccount(pAccount);

            return true;
        }

        [ConsoleCommand("delete")]
        public static bool CMD_ACCOUNT_DELETE(string[] Params)
        {
            var accountName = Params[0];

            if (AccountManager.GetAccountByName(accountName, out var pAccount))
            {
                if (pAccount.IsOnline)
                {
                    CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account is online! Account must be offline before deleting!");
                    return true;
                }
                ServerAccountMethods.SendAccountDelete(pAccount);
                return true;
            }

	        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account {0} not found!", accountName);
	        return true;
        }

        //Create <AccountName> <Password>
        [ConsoleCommand("Create")]
        public static bool CMD_ACCOUNT_CREATE(string[] Params)
        {
	        if (Params.Length < 2) return false;
	        var mAccountName = Params[0];

	        var password = Params[1];

	        if (password.Length > 16)
	        {
		        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Password length Max is 16");
		        return true;
	        }
	        var mResponse = AccountManager.CreateAccount(mAccountName, MD5Password.CalculateMD5Hash(password), out var mAccount);

	        switch (mResponse)
	        {
		        case AccountCreateResponse.NameTaken:
			        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "AccountName {0} is already in Use", mAccountName);
			        return true;

		        case AccountCreateResponse.IDOverflow:
			        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "AccountIDs are overflow");
			        return true;

		        case AccountCreateResponse.InternalError:
			        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Internal Account Error");
			        return true;

		        case AccountCreateResponse.SQLError:
			        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "SQL Error to create Account...", mAccountName);
			        return true;

		        case AccountCreateResponse.Success:
			        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account {0} created with password {1} Success!", mAccountName, password);
			        return true;
	        }
	        return false;
        }

        [ConsoleCommand("Set")]
        public static bool CMD_ACCOUNT_SET(string[] Params)
        {
	        if (Params.Length <= 2) return false;
	        var accountName = Params[1];

	        if (!AccountManager.GetAccountByName(accountName, out var pAccount))
	        {
		        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Account {0} not found!", accountName);
		        return true;
	        }

	        switch (Params[0].ToUpper())
	        {
		        case "ROLE" when (Params.Length == 3 && byte.TryParse(Params[2], out var roleID)):

			        pAccount.RoleID = roleID;

			        ServerAccountMethods.SendAccountUpdate(pAccount);
			        AccountManager.UpdateAccount(pAccount);

			        CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Set Account {0} Role to {1} ", accountName, roleID);
			        return true;

		        case "EMAIL" when (Params.Length == 3):
			        var pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

			        if (!Regex.IsMatch(Params[2], pattern))
			        {
				        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Email Address {0}", Params[2]);
				        return true;
			        }

			        pAccount.EMail = Params[2];

			        AccountManager.UpdateAccount(pAccount);
			        ServerAccountMethods.SendAccountUpdate(pAccount);
			        CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Set Account {0} EMail Address to {1} ", accountName, Params[2]);

			        return true;

		        case "ACTIVE" when (Params.Length == 3 && StringExtensions.ParseBool(Params[2], out var state)):

			        pAccount.IsActivated = state;

			        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters,
				        state ? "Activate Account {0}" : "Deactivate Account {0}", accountName);

			        AccountManager.UpdateAccount(pAccount);

			        return true;

		        case "PASSWORD" when (Params.Length == 3):

			        if (Params[2].Length > 16) // TODO: make this configurable
			        {
				        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Password Length the max Length is 16");
				        return true;
			        }

			        pAccount.Password = Params[2];

			        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Set Account {0} Password to {1} ", accountName, Params[2]);
			        AccountManager.UpdateAccount(pAccount);

			        return true;

		        case "REGION" when (Params.Length == 3):
			        if (!byte.TryParse(Params[2], out var regionID))
			        {
				        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid input use Account Set Region <AccountName> <RegionID>");
				        return true;
			        }

			        if (!Enum.TryParse(Params[2], out ClientRegion pRegion))
			        {
				        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid <RegionID>");
				        return true;
			        }
			        AccountManager.UpdateAccount(pAccount);
			        CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Set Account {0} Region to {1} ", accountName, pRegion);
			        return true;

		        default:
			        return false;
	        }
        }
    }
}