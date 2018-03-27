using DragonFiesta.Networking.Network.Session;
using DragonFiesta.World.Config;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Auth;
using DragonFiesta.World.ServerTask.Intern;
using System.Net.Sockets;

[ServerModule(ServerType.World, InitializationStage.Networking)]
public class InternLoginConnector : InternConnector
{
    public InternLoginConnector(Socket mSocket)
        : base(mSocket)
    {
        OnDisconnect += ClearConnection;
    }

    private void ClearConnection(object sender, SessionDisconnectArgs e)
        => ServerTaskManager.AddObject(new TASK_WORLD_SERVER_RECONNECT());

    [InitializerMethod]
    public static bool InitialConnect() => ConnectToServer();

    //first Connect
    private static bool ConnectToServer() => TryConnectTo<InternLoginConnector>(
        WorldConfiguration.Instance.ConnectToInfo.ConnectIP,
        WorldConfiguration.Instance.ConnectToInfo.ConnectPort);

    //Connect for Reconnecting
    public static bool Connect() => Instance.ConnectOnlyOne(
        WorldConfiguration.Instance.ConnectToInfo.ConnectIP,
        WorldConfiguration.Instance.ConnectToInfo.ConnectPort);



    public override void SendAuth() => AuthenticateMethods.SendAuthWorld(this);

    public override void SendMessage(IMessage pMessage, bool AddCallback = true)
    {
        base.SendMessage(pMessage, AddCallback);
    }
}