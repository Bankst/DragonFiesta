using DragonFiesta.Game.ServerConsole.Handler;
using DragonFiesta.Zone.Core;
using DragonFiesta.Zone.Game.Chat;
using System;

namespace DragonFiesta.Zone.ServerConsole.Server
{
    public sealed class ZoneShutdownHandler : ShutdownHandlerBase
    {
        public static bool IsInitialized() => Instance != null;
        private static ZoneShutdownHandler Instance { get; set; }
        public ZoneShutdownHandler(int ShutdownTime, string reason)
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

        public static bool UnInitial(string Reason)
        {
            if (!IsInitialized())
                return false;

            if (!ServerTaskManager.RemoveObject(Instance))
                return false;

            Instance = null;

            ZoneChat.LocalZoneNote($"Zone Server Shutdown Breaked { (Reason != null ? "Reason : " + Reason : "") } ");

            return true;
        }

        public static bool Initialize(int Seconds, string Reason)
        {
            if (IsInitialized())
                return false;

            Instance = new ZoneShutdownHandler(Seconds, Reason);

            ServerTaskManager.AddObject(Instance);

            ZoneChat.LocalZoneNote($"Zone Server Shutdown Initalized Reason : {Reason}");

            return true;
        }

        public override void FinalyShutdown() => ServerMain.InternalInstance.Shutdown();

        public override void ShutdownSequense_1Seconds(string Reason, TimeSpan RestTime)
            => ZoneChat.LocalZoneNote($"Zone Shutdown in {RestTime.TotalSeconds}");

        public override void ShutdownSequense_2Minutes(string Reason, TimeSpan RestTime)
            => ZoneChat.LocalZoneNote($"Zone Shutdown in {RestTime.TotalMinutes} Minutes And {RestTime.TotalSeconds} Seconds ");

        public override void ShutdownSequense_30Seconds(string Reason, TimeSpan RestTime)
           => ZoneChat.LocalZoneNote($"Zone Shutdown in {RestTime.TotalSeconds}");

        public override void ShutdownSequense_5Minutes(string Reason, TimeSpan RestTime)
        {
            ZoneChat.LocalZoneNote($"Zone Shutdown in {RestTime.TotalHours}  Hours {RestTime.TotalMinutes} Minutes And {RestTime.TotalSeconds} Seconds ");

            ZoneChat.LocalZoneNote($"Reason : {Reason}");
        }
    }
}
