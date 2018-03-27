using DragonFiesta.Networking.Network;
using System.Net.Sockets;

public class SessionDisconnectArgs : SessionEventArgs
{
    public SocketError Error { get; private set; }

    public SessionDisconnectArgs(SessionBase mSession, SocketError mError)
        : base(mSession)
    {
        Error = mError;
    }
}