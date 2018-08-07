using DragonFiesta.Utils.ServerConsole;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.Game.Zone;
using DragonFiesta.Zone.Network;

namespace DragonFiesta.Zone.ServerConsole.Title
{
    public class ZoneConsoleTitle : ConsoleTitle
    {
        private const string ZoneConstString = "DragonFiesta.Zone [{0}] Players [{1}/{2}] Remotes [{3}]";

        private int ZoneId => ZoneConfiguration.Instance == null ? -1 : ZoneConfiguration.Instance.ZoneID;

        private int PlayerCount => ZoneSessionManager.Instance == null ? -1 : ZoneSessionManager.Instance.CountOfSessions;

        private int MaxConnection => ZoneConfiguration.Instance == null ? -1 : ZoneConfiguration.Instance.ZoneServerInfo.MaxConnection;

        private int RemoteZoneCount => ZoneManager.RemoteZoneCount();

        public void Update()
        {
            Title = string.Format(ZoneConstString, ZoneId, PlayerCount, MaxConnection, RemoteZoneCount);
        }
    }
}