using System;
using System.Net.Sockets;

namespace DragonFiesta.Networking.Network.Session
{
    public abstract class InternConnector : InternSession
    {
        public InternConnector(Socket mSocket) : base(mSocket)
        {
        }

        public static InternConnector Instance { get; set; }

        public static bool GetIsConnected() => (Instance != null && Instance.IsConnected);

        public static bool GetIsAuthenticated => (Instance != null && Instance.IsReady);

        public abstract void SendAuth();

        protected internal static bool TryConnectTo<ConnectorType>(string IP, int port) where ConnectorType : InternConnector
        {
            try
            {
                Instance = (ConnectorType)Activator.CreateInstance(typeof(ConnectorType), new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
                Instance.TryConnectToLogin(IP, port);

                if (Instance.IsConnected)
                {
                    Instance.StartRecv();
                    Instance.SendAuth();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool ConnectOnlyOne(string host, int port) 
        {
            try
            {
               
                MSocket.Disconnect(true);
                if (!MSocket.BeginConnect(host, port, null, null).AsyncWaitHandle.WaitOne(5000, true))
                {
                    MSocket.Close();
                    return false;
                }

                StartRecv();
                SendAuth();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}