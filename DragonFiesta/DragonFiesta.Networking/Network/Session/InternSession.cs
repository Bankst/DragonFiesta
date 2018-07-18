using DragonFiesta.Messages;
using DragonFiesta.Networking.HandlerStores;
using DragonFiesta.Utils.Core;
using System.Net.Sockets;
using DragonFiesta.Networking.Network.Callbacks;

namespace DragonFiesta.Networking.Network
{
    public class InternSession : SessionBase
    {
        public InternSessionStateInfo SessionStateInfo { get; set; }

        public bool IsReady { get => SessionStateInfo.Authenticated && IsConnected; }//Seit c# 7.0 möglich

        protected TCPRecvCallBack<InternDataParser> RecvCallBack { get; private set; }

        private object ThreadLocker { get; set; }
        public InternSession(Socket mSocket) : base(mSocket)
        {
            ThreadLocker = new object();

            RecvCallBack = new TCPRecvCallBack<InternDataParser>(mSocket);
            RecvCallBack.DataParser.OnDataRecv += OnDataRecv;
            RecvCallBack.OnError += HandleSocketError;

            SessionStateInfo = new InternSessionStateInfo();

        }


        internal override void SendPacket<T>(T pPacket) =>  SendMessage((IMessage)pPacket);

        public override void StartRecv() => RecvCallBack.Start();

        protected virtual void HandleMessage(IMessage pMessage)
        {
            InternHandlerStore.Instance.HandleMessage(this, pMessage);
        }

        public virtual void SendMessage(IMessage pMessage, bool AddCallback = true)
        {
            lock (ThreadLocker)
            {
                if (!pMessage.GetType().IsSerializable)
                {
                    SocketLog.Write(SocketLogLevel.Warning, "Message {0} is not IsSerializable can not send", pMessage.GetType());
                    return;
                }

                if (pMessage is IExpectAnAnswer
                    && AddCallback)
                {

                    InternHandlerStore.Instance.AddCallBack(pMessage);
                }

                if (!IsConnected)
                {
                    Dispose();
                    return;
                }

                if (ServerMainDebug.DebugPackets)
                    SocketLog.Write(SocketLogLevel.Debug, "Send Message {0}", pMessage.GetType());


                BaseStateInfo.PacketsSent++;
            }

            Send(pMessage.MessageToBytes());
        }

        protected override void OnDataRecv(object sender, DataRecievedEventArgs e)
        {
            ThreadPool.AddCall(() =>
            {
                lock (ThreadLocker)
                {
                    IMessage msg = e.PacketData.BytesToMessage<dynamic>();

                    HandleMessage(msg);

                    BaseStateInfo.PacketsReceived++;
                }
            });

        }

        protected override void DisposeInternal()
        {
            if (IsConnected)
            {
                SessionStateInfo = null;

                RecvCallBack.Dispose();
                RecvCallBack = null;
            }

            base.DisposeInternal();
        }
    }
}