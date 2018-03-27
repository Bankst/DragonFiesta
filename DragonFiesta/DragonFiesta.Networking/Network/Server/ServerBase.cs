using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace DragonFiesta.Networking.Network
{
    public class ServerBase
    {
        private TcpListener listener;
        private bool isRunning;
        private CancellationTokenSource TokenSource;
        public ServerBase(int port)
        {
            var bindIP = IPAddress.None;

            if (PortInUse(port))
                throw new InvalidOperationException($"Can not find on Port {port }");

            listener = new TcpListener(IPAddress.Any, port);

            TokenSource = new CancellationTokenSource();
            Start();
        }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }

            return inUse;
        }

        private async void AcceptConnection(object delay)
        {
            try
            {
                while (isRunning)
                {
                    var clientSocket = await Task.Run(() =>
                   listener.AcceptSocketAsync(),
                    TokenSource.Token);

                    if (clientSocket != null)
                    {
                        SocketLog.Write(SocketLogLevel.Debug, $"Openned connection from { clientSocket.RemoteEndPoint }");
                        await DoWork(clientSocket);
                    }

                }
            }
            finally
            {
                listener.Stop();
            }
        }

        public virtual Task DoWork(Socket client)
        {
            return null;
        }

        private void Start()
        {
            try
            {
                listener.Start(20);
                SocketLog.Write(SocketLogLevel.Startup, "Listening on {0}", listener.LocalEndpoint);
                if (isRunning = listener.Server.IsBound)
                    new Thread(AcceptConnection).Start(5);
            }
            catch (Exception ex)
            {
                SocketLog.Write(SocketLogLevel.Exception, ex.ToString());
            }
        }

        public void Stop()
        {
            TokenSource.Cancel();
            isRunning = false;
        }
    }
}