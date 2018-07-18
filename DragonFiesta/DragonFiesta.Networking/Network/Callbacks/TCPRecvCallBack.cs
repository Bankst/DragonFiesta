using System;
using System.Net.Sockets;

namespace DragonFiesta.Networking.Network.Callbacks
{

//todo fix protocol parsing -.-
    public class TCPRecvCallBack<TDataParser> : IDisposable
        where TDataParser : DataParserBase
    {
        protected byte[] CurrentReceiveBuffer { get; private set; }

        protected int CurrentPositionInReceiveBuffer;

        private const int ReceivingBufferSize = ushort.MaxValue * 2;

        private Socket _mSocket;

        internal TDataParser DataParser { get; set; }

        public event EventHandler<SocketDisconnectArgs> OnError;

        internal void InvokeError(SocketError error, string msg = "") => OnError?.Invoke(this, new SocketDisconnectArgs(error, msg));

        public TCPRecvCallBack(Socket mSocket)
        {
            this._mSocket = mSocket;

            CurrentReceiveBuffer = new byte[ReceivingBufferSize];

            CurrentPositionInReceiveBuffer = 0;

            mSocket.ReceiveTimeout = 5000;
            mSocket.SendTimeout = 5000;

            DataParser = Activator.CreateInstance<TDataParser>();
        }

        public void Start()
        {
            BeginReceive();
        }

        internal void BeginReceive()
        {

            try
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += FinishReceive;
                args.SetBuffer(CurrentReceiveBuffer,
                    CurrentPositionInReceiveBuffer,
                    CurrentReceiveBuffer.Length - CurrentPositionInReceiveBuffer);

                if (!_mSocket.ReceiveAsync(args))
                {
                    FinishReceive(this, args);
                }
            }
            catch (Exception ex)
            {
                InvokeError(SocketError.SocketError, $"Error beginning receive: {ex}");
            }
        }

        protected virtual void FinishReceive(object sender, SocketAsyncEventArgs args)
        {
            var transfered = args.BytesTransferred;

            if (transfered < 1)
            {
                InvokeError(args.SocketError);
                return;
            }
            try
            {
	            DataParser.ParseNext(CurrentReceiveBuffer, ref CurrentPositionInReceiveBuffer, transfered);
            }
            catch (Exception e)
            {
                InvokeError(SocketError.SocketError, e.ToString());
            }
            finally
            {
                args.Dispose();
                BeginReceive();
            }
        }

        public void Dispose()
        {
            _mSocket = null;
            DataParser = null;
        }
    }
}