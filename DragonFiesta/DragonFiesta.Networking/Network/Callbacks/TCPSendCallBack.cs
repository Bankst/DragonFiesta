using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace DragonFiesta.Networking.Network
{
    public class TCPSendCallBack : IDisposable
    {
        public const int SendingBufferSize = ushort.MaxValue;

        private ConcurrentQueue<byte[]> SendBuffer;
        private int IsSending;

        private object writeState;
        private byte[] currentSendBuffer;

        private Socket pSocket;

        public event EventHandler<SocketDisconnectArgs> OnSendError;

        private void InvokeError(SocketError Error, string msg = "") => OnSendError?.Invoke(this, new SocketDisconnectArgs(Error, msg));

        public TCPSendCallBack(Socket BindSocket)
        {
            pSocket = BindSocket;
            SendBuffer = new ConcurrentQueue<byte[]>();
            currentSendBuffer = new byte[SendingBufferSize];

            writeState = new object();
        }

        public void Send(byte[] Data)
        {
            SendBuffer.Enqueue(Data);

            if (Interlocked.CompareExchange(ref IsSending, 1, 0) == 0)
            {
                BeginSend();
            }
        }

        private void BeginSend()
        {
            if (!pSocket.Connected) return;

            try
            {
                if (SendBuffer.TryPeek(out byte[] data))
                {
                    var args = new SocketAsyncEventArgs();
                    args.Completed += FinishSend;
                    args.SetBuffer(data, 0, data.Length);

                    if (!pSocket.SendAsync(args))
                    {
                        FinishSend(this, args);
                    }
                }
                else
                {
                    Interlocked.Exchange(ref IsSending, 0);
                }
            }
            catch (Exception ex)
            {
                InvokeError(SocketError.SocketError, $"Error beginning send: {ex}");
            }
        }

        private void FinishSend(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                var transfered = args.BytesTransferred;

                if (transfered < 1 && args.SocketError != SocketError.Success)
                {
                    InvokeError(args.SocketError);
                    return;
                }

                //check if everything has been send
                if (SendBuffer.TryPeek(out byte[] data))
                {
                    if (transfered >= data.Length)
                    {
                        //remove last buffer for allowing to send next buffer
                        SendBuffer.TryDequeue(out data);
                    }
                }
            }
            catch
            {
                InvokeError(args.SocketError);
            }
            finally
            {
                args.Dispose();
                BeginSend();
            }
        }

        public void Dispose()
        {
            SendBuffer = null;
            writeState = null;
            currentSendBuffer = null;
            pSocket = null;
        }
    }
}