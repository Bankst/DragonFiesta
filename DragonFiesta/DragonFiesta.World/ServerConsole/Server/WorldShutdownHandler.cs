using DragonFiesta.Game.ServerConsole.Handler;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Zone;
using System;

namespace DragonFiesta.World.ServerConsole.Server
{
    public sealed class WorldShutdownHandler : ShutdownHandlerBase
    {
        public static bool IsInitialized() => Instance != null;
        private static WorldShutdownHandler Instance { get; set; }

        public WorldShutdownHandler(int ShutdownTime, string reason)
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


            WorldChat.ServerAnnounce($"Server Shutdown Breaked  { (Reason != null ? "Reason : " + Reason : "") } ");
            EngineLog.Write(EngineLogLevel.Warning, "Gloabal Server Shutdown Breaked");
            Instance = null;

            return true;
        }

        public static bool Initialize(int Seconds, string Reason)
        {
            if (IsInitialized())
                return false;

            Instance = new WorldShutdownHandler(Seconds, Reason);

            WorldChat.ServerAnnounce($"Server Shutdown Intialized Reason : {Reason}");
            EngineLog.Write(EngineLogLevel.Warning, "Gloabal Server Shutdown initialized {0}", Reason);
            ServerTaskManager.AddObject(Instance);

            return true;
        }

        public override void FinalyShutdown()
        {
            ZoneMethods.BroadCastServerShutdown();

            //Shutdown Zone...
            ServerMain.InternalInstance.Shutdown();
        }


        public override void ShutdownSequense_1Seconds(string Reason, TimeSpan RestTime)
            => WorldChat.ServerAnnounce($"Server Shutdown in {(short)RestTime.TotalSeconds}");

        public override void ShutdownSequense_2Minutes(string Reason, TimeSpan RestTime)
            => WorldChat.ServerAnnounce($"Zone Shutdown in {RestTime.TotalMinutes} Minutes And {RestTime.TotalSeconds} Seconds ");

        public override void ShutdownSequense_30Seconds(string Reason, TimeSpan RestTime)
        {
            WorldChat.ServerAnnounce($"Zone Shutdown in {RestTime.TotalSeconds}");
            EngineLog.Write(EngineLogLevel.Info, "Shutdiwn in {0}", RestTime.TotalSeconds);
        }
        public override void ShutdownSequense_5Minutes(string Reason, TimeSpan RestTime)
        {
            WorldChat.ServerAnnounce($"Zone Shutdown in {RestTime.TotalHours}  Hours {RestTime.TotalMinutes} Minutes And {RestTime.TotalSeconds} Seconds ");

            WorldChat.ServerAnnounce($"Reason : {Reason}");
        }
    }
}
