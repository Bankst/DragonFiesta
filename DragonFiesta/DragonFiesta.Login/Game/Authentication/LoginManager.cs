using DragonFiesta.Login.Network;
using DragonFiesta.Utils.ServerTask;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Login.Game.Authentication
{
    [ServerModule(ServerType.Login, InitializationStage.Logic)]
    public class LoginManager
    {
        public static LoginManager Instance { get; set; }

        private ExpireAbleManager LoggedAccounts;

        private ConcurrentDictionary<ushort, AuthLogin> LoginsBySessionId;

        [InitializerMethod]
        public static bool Initialsize()
        {
            try
            {
                Instance = new LoginManager();

                EngineLog.Write(EngineLogLevel.Startup, "LoginManager initiasize");

                return true;
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed Start LoginManagaer", ex);
                return false;
            }
        }

        private LoginManager()
        {
            LoginsBySessionId = new ConcurrentDictionary<ushort, AuthLogin>();
            LoggedAccounts = new ExpireAbleManager((int)ServerTaskTimes.ACCOUNT_LOGIN_TIMEOUT_CHECK);
        }

        public bool Add(LoginSession pSession)
        {
            AuthLogin AuthClient = new AuthLogin(pSession.BaseStateInfo.SessiondId);
            if (LoginsBySessionId.TryAdd(pSession.BaseStateInfo.SessiondId, AuthClient))
            {
                LoggedAccounts.AddObject(AuthClient);
                return true;
            }
            return false;
        }

        public bool TryGetLogin(ushort SessionId, out AuthLogin Login)
        {
            if (LoginsBySessionId.TryRemove(SessionId, out Login))
            {
                LoggedAccounts.RemoveObject(Login);
                return true;
            }

            return false;
        }
    }
}