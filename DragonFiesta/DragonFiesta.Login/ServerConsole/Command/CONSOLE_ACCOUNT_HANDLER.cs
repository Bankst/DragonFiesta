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
            bool BanState = false;
            long BanTime = 0;
            if (Params.Length == 0 || Params.Length >= 2 && StringExtensions.ParseBool(Params[1], out BanState) & !long.TryParse(Params[1], out BanTime))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid input Use Account ban <Accountname> <bool> or <BanTime> <Reason>");
                return true;
            }

            if (!AccountManager.GetAccountByName(Params[0], out Account pAccount))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account {0} Not found!", Params[0]);
                return true;
            }
            //GetReason
            string Reason = string.Empty;
            if (Params.Length == 2)
                Reason = "Banned by Console No Reason";
            else
                Reason = String.Join(" ", 3, Params.Length);

            if (BanTime > 1)
            {
                pAccount.BanTime = BanTime;
                pAccount.BanDate = DateTime.Now;
                pAccount.BanReason = Reason;
                CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Ban Account {0} Success!!", pAccount.Name);
            }
            else
            {
                if (BanState)
                {
                    pAccount.BanTime = int.MaxValue;// yeahr its better unban in 31k Years :D
                    pAccount.BanDate = DateTime.Now;
                    pAccount.BanReason = Reason;
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
            string AccountName = Params[0];

            if (AccountManager.GetAccountByName(AccountName, out Account pAccount))
            {
                if (pAccount.IsOnline)
                {
                    CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account ist Online!! Mus be first Logout before Delete");
                    return true;
                }
                ServerAccountMethods.SendAccountDelete(pAccount);
                return true;
            }
            else
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account {0} not found!", AccountName);
                return true;
            }
        }

        //Create <AccountName> <Password>
        [ConsoleCommand("Create")]
        public static bool CMD_ACCOUNT_CREATE(string[] Params)
        {
            if (Params.Length >= 2)
            {
                string mAccountName = Params[0];

                string Password = Params[1];

                if (Password.Length > 16)
                {
                    CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Password length Max is 16");
                    return true;
                }
                AccountCreateResponse mResponse = AccountManager.CreateAccount(mAccountName, MD5Password.CalculateMD5Hash(Password), out Account mAccount);

                switch (mResponse)
                {
                    case AccountCreateResponse.NameTaken:
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "AccountName {0} is Alredy in Use", mAccountName);
                        return true;

                    case AccountCreateResponse.IDOverflow:
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "AccountIDs Are overflow");
                        return true;

                    case AccountCreateResponse.InternalError:
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Internal Account Error");
                        return true;

                    case AccountCreateResponse.SQLError:
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "SQL Error to create Account...", mAccountName);
                        return true;

                    case AccountCreateResponse.Success:
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Account {0} createt with password {1} Success!", mAccountName, Password);
                        return true;
                }
            }
            return false;
        }

        [ConsoleCommand("Set")]
        public static bool CMD_ACCOUNT_SET(string[] Params)
        {
            if (Params.Length > 2)
            {
                string AccountName = Params[1];

                if (!AccountManager.GetAccountByName(AccountName, out Account pAccount))
                {
                    CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Account {0} not found!", AccountName);
                    return true;
                }

                switch (Params[0].ToUpper())
                {
                    case "ROLE" when (Params.Length == 3 && byte.TryParse(Params[2], out byte RoleID)):

                        pAccount.RoleID = RoleID;

                        ServerAccountMethods.SendAccountUpdate(pAccount);
                        AccountManager.UpdateAccount(pAccount);

                        CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Set Account {0} Role to {1} ", AccountName, RoleID);
                        return true;

                    case "EMAIL" when (Params.Length == 3):
                        string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

                        if (!Regex.IsMatch(Params[2], pattern))
                        {
                            CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Email Adress {0}", Params[2]);
                            return true;
                        }

                        pAccount.EMail = Params[2];

                        AccountManager.UpdateAccount(pAccount);
                        ServerAccountMethods.SendAccountUpdate(pAccount);
                        CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Set Account {0} EMail Adress to {1} ", AccountName, Params[2]);

                        return true;

                    case "ACTIVE" when (Params.Length == 3 && StringExtensions.ParseBool(Params[2], out bool State)):

                        pAccount.IsActivated = State;

                        if (State)
                        {
                            CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Activate Account {0}", AccountName);
                        }
                        else
                        {
                            CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Deactivate Account {0}", AccountName);
                        }

                        AccountManager.UpdateAccount(pAccount);

                        return true;

                    case "PASSWORD" when (Params.Length == 3):

                        if (Params[2].Length > 16)
                        {
                            CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Password Lenght the max Lenght is 16");
                            return true;
                        }

                        pAccount.Password = Params[2];

                        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Set Account {0} Password to {1} ", AccountName, Params[2]);
                        AccountManager.UpdateAccount(pAccount);

                        return true;

                    case "REGION" when (Params.Length == 3):
                        byte RegionID;
                        if (!byte.TryParse(Params[2], out RegionID))
                        {
                            CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid input use Account Set Region <AccountName> <RegionID>");
                            return true;
                        }

                        ClientRegion pRegion;
                        if (!Enum.TryParse(Params[2], out pRegion))
                        {
                            CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid <RegionID>");
                            return true;
                        }
                        AccountManager.UpdateAccount(pAccount);
                        CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Set Account {0} Region to {1} ", AccountName, pRegion);
                        return true;

                    default:
                        return false;
                }
            }
            return false;
        }
    }
}