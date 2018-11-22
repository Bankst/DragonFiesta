namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_USER_XTRAP_ACK : NetworkMessage
    {
        public PROTO_NC_USER_XTRAP_ACK(bool success) : base(NetworkCommand.NC_USER_XTRAP_ACK)
        {
            Write(success);
        }
    }
}
