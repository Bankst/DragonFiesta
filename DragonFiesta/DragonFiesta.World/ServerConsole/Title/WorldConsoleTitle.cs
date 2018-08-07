using DragonFiesta.Utils.ServerConsole;
using DragonFiesta.World.Config;
using DragonFiesta.World.Game.Zone;
using DragonFiesta.World.Network;

namespace DragonFiesta.World.ServerConsole.Title
{
    public class WorldConsoleTitle : ConsoleTitle
    {
        private const string TitleString = "DragonFiesta.World ID [{0}] Zones [{1}/{2}] Players [{3}/{4}]";

        private int PlayerCount => WorldSessionManager.Instance == null ? -1 : WorldSessionManager.Instance.CountOfSessions;

        private int WorldId => WorldConfiguration.Instance == null ? -1 : WorldConfiguration.Instance.WorldID;

        private int MaxPlayers => WorldConfiguration.Instance == null ? -1 : WorldConfiguration.Instance.ServerInfo.MaxConnection;

        private int ActiveZone => ZoneManager.CountOfSessions();

        private int MaxZones => WorldConfiguration.Instance == null ? -1 : WorldConfiguration.Instance.InternalServerInfo.MaxConnection;

        public void Update() => Title = string.Format(TitleString, WorldId, ActiveZone, MaxZones, PlayerCount, MaxPlayers);
    }
}