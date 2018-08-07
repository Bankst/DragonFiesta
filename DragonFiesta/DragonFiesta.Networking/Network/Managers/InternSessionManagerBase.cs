using System;

namespace DragonFiesta.Networking.Network
{
    public class InternSessionManagerBase<SessionType> : SessionBaseManager<InternSession>
        where SessionType : InternSession
    {
        public static new InternSessionManagerBase<SessionType> Instance
        {
            get => _Instance as InternSessionManagerBase<SessionType>;
            set => _Instance = value;
        }
        public InternSessionManagerBase(ushort MaxSessions) : base(MaxSessions)
        {
        }

        public override void Broadcast<T>(T Packet) => ClientAction((client) => client.SendMessage((IMessage)Packet));

        public override void Broadcast<T>(T Packet, Predicate<InternSession> Match) => ClientAction((client) => client.SendMessage((IMessage)Packet), Match);
    }
}