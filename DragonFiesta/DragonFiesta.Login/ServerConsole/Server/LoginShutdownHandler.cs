using DragonFiesta.Game.ServerConsole.Handler;
using DragonFiesta.Login.Core;
using System;

namespace DragonFiesta.Login.ServerConsole.Server
{
    public sealed class LoginShutdownHandler : ShutdownHandlerBase
    {
        public static bool IsInitialized() => Instance != null;
        private static LoginShutdownHandler Instance { get; set; }

        public LoginShutdownHandler(int ShutdownTime, string reason) 
            : base(ShutdownTime, reason)
        {
        }

        public static bool Update(int Seconds, string Reason)
        {
            if (!IsInitialized())
                return false;

            Instance.UpdateTime(Seconds, Reason);
            return true;
        }

        public static bool UnInitial()
        {
            if (!IsInitialized())
                return false;

            if (!ServerTaskManager.RemoveObject(Instance))
                return false;


            EngineLog.Write(EngineLogLevel.Warning, "Shutdown Cancel");

            Instance = null;

            return true;
        }

        public static bool Initialize(int Seconds, string Reason)
        {
            if (IsInitialized())
                return false;

            Instance = new LoginShutdownHandler(Seconds, Reason);

            ServerTaskManager.AddObject(Instance);

            return true;
        }

        public override void FinallyShutdown() => ServerMain.InternalInstance.Shutdown();
        
        private void DisplayMessage(TimeSpan RestTime)
        {
            EngineLog.Write(EngineLogLevel.Info, "LoginServer Shutdown in {0} Hours {1] Minutes {2} Seconds", RestTime.TotalHours, RestTime.TotalMinutes, RestTime.TotalSeconds);
        }
        public override void ShutdownSequense_1Seconds(string Reason, TimeSpan RestTime)
        {
            EngineLog.Write(EngineLogLevel.Info, "LoginServer Shutdown in {0} Seconds", RestTime.TotalSeconds);
        }

        public override void ShutdownSequense_2Minutes(string Reason, TimeSpan RestTime) => DisplayMessage(RestTime);

        public override void ShutdownSequense_30Seconds(string Reason, TimeSpan RestTime) => DisplayMessage(RestTime);

        public override void ShutdownSequense_5Minutes(string Reason, TimeSpan RestTime) => DisplayMessage(RestTime);
    }
}
