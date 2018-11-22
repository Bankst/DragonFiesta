namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_AVATAR_CREATEFAIL_ACK : NetworkMessage
    {
        public PROTO_NC_AVATAR_CREATEFAIL_ACK(ushort error) : base(NetworkCommand.NC_AVATAR_CREATEFAIL_ACK)
        {
            Write(error);
        }
    }
}
