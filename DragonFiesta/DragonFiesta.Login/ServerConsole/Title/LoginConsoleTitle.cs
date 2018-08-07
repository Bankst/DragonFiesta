using DragonFiesta.Login.Config;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Login.InternNetwork;
using DragonFiesta.Login.Network;
using DragonFiesta.Utils.ServerConsole;

namespace DragonFiesta.Login.ServerConsole.Title
{
    public class LoginConsoleTitle : ConsoleTitle
    {
        private static string Worldstring = "DragonFiesta.Login Worlds [{0}/{1}] Connections [{2}/{3}]";

        private byte WorldCount => InternWorldSessionManager.Instance != null ? (byte)InternWorldSessionManager.Instance.CountOfSessions : (byte)0;

        private byte MaxWorldCount => WorldManager.Instance != null ? (byte)WorldManager.Instance.WorldList.Count : (byte)0;

        private int ConnectionCount => LoginSessionManager.Instance != null ? LoginSessionManager.Instance.CountOfSessions : (byte)0;

        private int MaxConnection => LoginConfiguration.Instance != null ? LoginConfiguration.Instance.GameServerInfo.MaxConnection : (byte)0;

        public void Update() => Title = string.Format(Worldstring, WorldCount, MaxWorldCount, ConnectionCount, MaxConnection);
    }
}