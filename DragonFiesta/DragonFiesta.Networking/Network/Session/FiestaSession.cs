using DragonFiesta.Networking.HandlerStores;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Utils;
using DragonFiesta.Utils.Core;
using System;
using System.Net.Sockets;

namespace DragonFiesta.Networking.Network
{
    public class FiestaSession<TSession> : SessionBase
        where TSession : FiestaSession<TSession>
    {
        public bool IsTransfered() => (GameStates.Authenticated && GameStates.IsTransferring);

        public FiestaSessionStateInfo GameStates { get; private set; }



        private TCPRecvCallBack<FiestaDataParser> RecvCallBack;

        private FiestaSharkDumper DebugPackets { get; set; }

        public FiestaSession(ClientRegion mRegion, Socket mSocket) : base(mSocket)
        {
            
            GameStates = new FiestaSessionStateInfo()
            {
                Region = mRegion,
            };

            if (ServerMainDebug.DumpPacket)
                DebugPackets = new FiestaSharkDumper(BaseStateInfo.SessionId);

           OnDispose += FiestaSession_OnDisconnect;
        }

        private void FiestaSession_OnDisconnect(object sender, SessionEventArgs e)
            => FiestaSessionManagerBase<TSession>.Instance.RemoveSession(BaseStateInfo.SessionId);

        public void Start()
        {
            StartRecv();
            SetXorKeyPositionToRandom();
        }

        public override void StartRecv()
        {
            RecvCallBack = new TCPRecvCallBack<FiestaDataParser>(MSocket);
            RecvCallBack.DataParser.OnDataRecv += OnDataRecv;

            RecvCallBack.OnError += HandleSocketError;

            RecvCallBack.Start();
        }

        protected override void OnDataRecv(object sender, DataRecievedEventArgs e)
        {
            if (GameStates?.Crypto == null)//Checking Sending Handshake
            {
                Dispose();
                return;
            }

            ThreadPool.AddCall(() =>
            {
                if (IsDisposed)
                    return;

                var Packet = new FiestaPacket(GameStates.Crypto.Decrypt(e.PacketData, 0, e.PacketData.Length));

                SocketLog.Write(SocketLogLevel.Debug, "Read FiestaPacket H{0} T{1}", Packet.Header, Packet.Type);

                if (DebugPackets != null)
                    DebugPackets.DumpPacket(Packet, true);

                FiestaHandlerStore.Instance.HandlePacket(Packet, this);

                BaseStateInfo.PacketsReceived++;

            });
        }

        public void SetXorKeyPositionToRandom()
        {
            Random rng = new Random();
            ushort position = (ushort)rng.Next(0, 499);
            SetXorKeyPosition(position);
        }

        public void SetXorKeyPosition(ushort position)
        {
            GameStates.Crypto = new Cryptography.FiestaCryptoProvider(position);

            SendSetXorKeyPosition(position);
        }

        public void SendPacket(FiestaPacket pPacket)
        {

            if (!IsConnected)
            {
                Dispose();
                return;
            }

            if (DebugPackets != null)
                DebugPackets.DumpPacket(pPacket, false);

            Send(pPacket.GetPacketBytes());

            BaseStateInfo.PacketsSent++;

        }

        private void SendSetXorKeyPosition(ushort position)
        {
            using (FiestaPacket packetWriter = new FiestaPacket(Handler02Type._Header, Handler02Type.SMSG_MISC_SEED_ACK))
            {
                packetWriter.Write<UInt32>(position);
                SendPacket(packetWriter);
            }

            SocketLog.Write(SocketLogLevel.Debug, "Connection {0} set xor key position to ", position);
        }

        internal override void SendPacket<T>(T pPacket) => SendPacket(pPacket);

        protected override void DisposeInternal()
        {
            RecvCallBack = null;
            GameStates = null;
            DebugPackets = null;

            base.DisposeInternal();
        }
    }
}