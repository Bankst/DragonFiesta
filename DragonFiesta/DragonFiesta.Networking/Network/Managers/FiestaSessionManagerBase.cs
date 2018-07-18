using System;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Networking.Network
{
    public class FiestaSessionManagerBase<TSession> : SessionBaseManager<TSession>
     where TSession : FiestaSession<TSession>
    {

        private static new FiestaSessionManagerBase<TSession> Instance
        {
            get => _Instance as FiestaSessionManagerBase<TSession>;
        }

        protected FiestaSessionManagerBase(ushort MaxSessions) : base(MaxSessions)
        {
            ThreadLocker = new object();


        }


        public override void Broadcast<T>(T Packet) => ClientAction((client) => client.SendPacket((Packet as FiestaPacket)));

        public override void Broadcast<T>(T Packet, Predicate<TSession> Match) => ClientAction((client) => client.SendPacket((Packet as FiestaPacket)), Match);
    }
}