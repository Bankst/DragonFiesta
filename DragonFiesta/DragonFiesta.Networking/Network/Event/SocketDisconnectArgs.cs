using System;
using System.Net.Sockets;

namespace DragonFiesta.Networking.Network
{
    public class SocketDisconnectArgs : EventArgs
    {
        public SocketError Error { get; private set; }

        public string Message { get; private set; }

        public SocketDisconnectArgs(SocketError Error, string msg = "")
        {
            this.Message = msg;
            this.Error = Error;
        }

        ~SocketDisconnectArgs()
        {
            Message = null;
        }
    }
}