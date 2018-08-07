using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Network;
using DragonFiesta.Utils.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Login.Game.Accounts
{
    [GameServerModule(ServerType.Login, GameInitalStage.Logic)]
    public class AccountManager
    {
        private static object ThreadLocker;

        [InitializerMethod]
        public static bool OnStart()
        {
            ThreadLocker = new object();

            CleanUp();

            return true;
        }

        private static void CleanUp()
        {
            DB.RunSQL(DatabaseType.Login, "UPDATE Accounts SET IsOnline = 0");
        }

        #region Database

        public static AccountDeleteResponse DeleteAccount(int AccountID)
        {
            try
            {
                if (LoginSessionManager.Instance.GetAccount(AccountID, out LoginSession mSession))
                {
                    mSession.Dispose();
                }

                DB.RunSQL(DatabaseType.Login, "DELETE FROM Accounts WHERE ID=" + AccountID + "");
                return AccountDeleteResponse.Success;
            }
            catch
            {
                return AccountDeleteResponse.SQLError;
            }
        }

        public static AccountCreateResponse CreateAccount(
            string Name,
            string Password,
            out Account Account,
            string EMail = "DragonFiesta@DF.de",
            string UserIP = "0.0.0.0",
            bool IsActivated = false,
            bool IsOnline = false,
            byte RoleID = 0)
        {
            Account = null;
            try
            {
                lock (ThreadLocker)
                {
                    //get some variables
                    var time = ServerMainBase.InternalInstance.CurrentTime;
                    var BanDate = new DateTime(time.Time.Year, time.Time.Date.Month, time.Time.Date.Day, 0, 0, 0);
                    long BanTime = 0;
                    // add to database
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.Login))
                    {
                        cmd.CreateStoredProcedure("dbo.Account_Insert");
                        cmd.SetParameter("@pName", Name);
                        cmd.SetParameter("@pEMail", EMail);
                        cmd.SetParameter("@pPassword", Password);
                        cmd.SetParameter("@pDate", time.Time);
                        cmd.SetParameter("@pUserIP", UserIP);
                        cmd.SetParameter("@pIsActivated", IsActivated);
                        cmd.SetParameter("@pBanDate", BanDate);
                        cmd.SetParameter("@pBanTime", BanTime);
                        cmd.SetParameter("@pIsOnline", IsOnline);
                        cmd.SetParameter("@pRoleID", RoleID);
                        var idParam = cmd.SetParameter("@pID", SqlDbType.Int, ParameterDirection.Output);

                        var res = (AccountCreateResponse)(int)cmd.ExecuteScalar();
                        if (res == AccountCreateResponse.Success)
                        {
                            Account = new Account((int)idParam.Value, Name, EMail, Password, time.Time, UserIP, IsActivated, BanDate, BanTime, IsOnline, RoleID);
                        }
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error creating account:");

                return AccountCreateResponse.InternalError;
            }
        }

        public static bool GetAccountByID(int ID, out Account Account)
        {
            lock (ThreadLocker)
            {
                return LoadAccountFromDatabase("ID = @pID", out Account, new SqlParameter("@pID", ID));
            }
        }

        public static bool GetAccountByName(string Name, out Account Account)
        {
            lock (ThreadLocker)
            {
                return LoadAccountFromDatabase("Name = @pName", out Account, new SqlParameter("@pName", Name));
            }
        }

        public static bool GetAccountByEMail(string EMail, out Account Account)
        {
            lock (ThreadLocker)
            {
                return LoadAccountFromDatabase("EMail = @pEMail", out Account, new SqlParameter("@pEMail", EMail));
            }
        }

        public static void UpdateAccountState(Account Account)
        {
            try
            {
                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.Login))
                    {
                        cmd.CreateStoredProcedure("dbo.Account_UpdateState");
                        cmd.SetParameter("@pID", Account.ID);
                        cmd.SetParameter("@pIsOnline", Account.IsOnline);
                        cmd.SetParameter("@pLastLogin", Account.LastLogin);
                        cmd.SetParameter("@pLastIP", Account.LastIP);
                        cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error updating account state:");
            }
        }

        public static void UpdateAccount(Account Account)
        {
            try
            {
                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.Login))
                    {
                        cmd.CreateStoredProcedure("dbo.Account_Update");
                        cmd.SetParameter("@pID", Account.ID);
                        cmd.SetParameter("@pPassword", Account.Password);
                        cmd.SetParameter("@pEMail", Account.EMail);
                        cmd.SetParameter("@pBanDate", Account.BanDate);
                        cmd.SetParameter("@pBanTime", Account.BanTime);
                        cmd.SetParameter("@pBanReason", Account.BanReason);
                        cmd.SetParameter("@pIsActivated", Account.IsActivated);
                        cmd.SetParameter("@pIP", Account.LastIP);
                        cmd.SetParameter("@pRoleID", Account.RoleID);
                        cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error updating account");
            }
        }

        private static bool LoadAccountFromDatabase(string Where, out Account Account, params SqlParameter[] Parameters)
        {
            Account = null;
            try
            {
                lock (ThreadLocker)
                {
                    SQLResult pResult = DB.Select(DatabaseType.Login, " SELECT TOP 1 * FROM Accounts WHERE " + Where, Parameters);

                    if (pResult.Count != 1)
                    {
                        return false;
                    }
                    Account = new Account(pResult, 0);
                    return true;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error loading account from database:");
                return false;
            }
        }

        #endregion Database
    }
}