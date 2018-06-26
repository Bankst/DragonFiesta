using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DragonFiesta.Networking.Network
{
    public abstract class SessionBase : IDisposable
    {
        private TCPSendCallBack SendCallBack;

        protected internal Socket MSocket { get; set; }

        public event EventHandler<SessionDisconnectArgs> OnDisconnect;
        public event EventHandler<SessionEventArgs> OnDispose;


        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;


        public ClientStateInfo BaseStateInfo { get; internal set; }
        public bool IsConnected { get { return MSocket != null && MSocket.Connected; } }

        public string ConnectionInfo { get; set; }


        public string GetIP()
        {
            return ConnectionInfo != null ? ConnectionInfo.Split(':')[0] : "0.0.0.0";
        }

        public SessionBase(Socket mSocket)
        {
            this.MSocket = mSocket;
            BaseStateInfo = new ClientStateInfo();

            SendCallBack = new TCPSendCallBack(mSocket);

            //Events

            ConnectionInfo = GetClientInfo();

            SendCallBack.OnSendError += HandleSocketError;

            OnDispose += (e, arg) => Console.WriteLine("Dispose");


        }

        ~SessionBase()
        {
            Dispose();
        }

        internal void HandleSocketError(object sender, SocketDisconnectArgs e)
        {

            switch (e.Error)
            {
                case SocketError.Success://DC By Receive
                case SocketError.ConnectionReset:

                    SocketLog.Write(SocketLogLevel.Debug, $"Connection Closed: { MSocket.RemoteEndPoint }");
                    break;
                case SocketError.NotConnected:
                    MSocket.Close();
                    return;
                case SocketError.ConnectionAborted:
                    SocketLog.Write(SocketLogLevel.Startup, $"Connection Refused: { MSocket.RemoteEndPoint }");
                    break;
                case SocketError.TimedOut:
                    SocketLog.Write(SocketLogLevel.Debug, $"Connection Timed Out: { MSocket.RemoteEndPoint }");
                    break;
                default:
                    SocketLog.Write(SocketLogLevel.Warning, $"Unhandle SocketError : {e.Error} : message : {e.Message}");
                    break;

                case SocketError.SocketError:

                    SocketLog.Write(SocketLogLevel.Exception, $"Unknown SocketError {e.Message}");
                    break;
            }

            OnDisconnect?.Invoke(this, new SessionDisconnectArgs(this, e.Error));

            MSocket.ShutdownSafely();
            Dispose();

        }

        public string GetClientInfo()
        {
            var ipEndPoint = (MSocket.RemoteEndPoint as IPEndPoint);
            return ipEndPoint != null ? ipEndPoint.Address + ":" + ipEndPoint.Port : "";
        }


        public abstract void StartRecv();

        protected virtual void Send(byte[] data) => SendCallBack.Send(data);

        protected abstract void OnDataRecv(object sender, DataRecievedEventArgs m);

        internal abstract void SendPacket<T>(T pPacket) where T : class;

        internal void TryConnectToLogin(string host, int port, int tryCount = 0)
        {
            try
            {
                EngineLog.Write(EngineLogLevel.Startup, $"Connect To {host}:{port}");
                MSocket.Connect(host, port);
            }
            catch (Exception e)
#if DEBUG
             when (tryCount >= 50)//Neeed to connect in debug mode...
#else
             when(tryCount >= 50)
#endif
            {
                // we already tried 5 time
                EngineLog.Write(EngineLogLevel.Exception, $"Failed to connect to after {tryCount} tries");
                EngineLog.Write(EngineLogLevel.Exception, "Could not connect to server! Shutdown...", e);
            }
            catch   // if no "when"-clauses filter the exception out
            {
                // we haven't tried 5 times yet
                EngineLog.Write(EngineLogLevel.Exception, $"Try {tryCount} to connect to failed, trying again...");
                TryConnectToLogin(host, port, tryCount + 1);
            }
        }
        protected virtual void DisposeInternal() { }
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {

                OnDispose?.Invoke(this, new SessionEventArgs(this));
                OnDispose = null;

                if (IsConnected)
                {
                    MSocket.Kill();

                    SendCallBack.Dispose();
                    SendCallBack = null;

                }

                DisposeInternal();

            }
        }
    }
}