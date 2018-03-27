using System;
using System.Net.Sockets;

public static class SocketExtensions
{
    public static void ShutdownSafely(this Socket Socket, SocketShutdown how = SocketShutdown.Both)
    {
        try
        {
            Socket.Shutdown(how);
        }
        catch (Exception)
        {
        }
    }

    public static void Kill(this Socket Socket)
    {
        Socket.ShutdownSafely();
        Socket.Dispose();
    }
}