using DragonFiesta.Networking.Network.Session;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Auth;
using DragonFiesta.Zone.ServerTask.Intern;
using System.Net.Sockets;
using DragonFiesta.Zone.Game.Zone;
using DragonFiesta.Zone.Game.Maps;
using DragonFiesta.Zone.Game.Maps.Object;

[ServerModule(ServerType.Zone, InitializationStage.Networking)]
public class InternWorldConnector : InternConnector
{
    public InternWorldConnector(Socket mSocket)
        : base(mSocket)
    {
        OnDisconnect += (Sender, Arg) => DisposeForReconnect();
    }

    public static void DisposeForReconnect()
    {
        MapManager.Dispose();
        ZoneManager.Dispose();
        ServerTaskManager.AddObject(new TASK_ZONE_SERVER_RECONNECT());
    }

    [InitializerMethod]
    public static bool InitialConnect() => ConnectToServer();

    //first Connect
    private static bool ConnectToServer() => TryConnectTo<InternWorldConnector>(
        ZoneConfiguration.Instance.WorldConnectInfo.ConnectIP,
         ZoneConfiguration.Instance.WorldConnectInfo.ConnectPort);

    //Connect for Reconnecting
    public static bool Connect() => Instance.ConnectOnlyOne(
         ZoneConfiguration.Instance.WorldConnectInfo.ConnectIP,
         ZoneConfiguration.Instance.WorldConnectInfo.ConnectPort);

    public override void SendAuth() => AuthenticateMethods.SendAuthZone(this);
}