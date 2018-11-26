namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_AVATAR_ERASEFAIL_ACK : NetworkMessage
    {
        public PROTO_NC_AVATAR_ERASEFAIL_ACK(ushort error) : base(NetworkCommand.NC_AVATAR_ERASEFAIL_ACK)
        {
            Write(error);
        }
    }
}
