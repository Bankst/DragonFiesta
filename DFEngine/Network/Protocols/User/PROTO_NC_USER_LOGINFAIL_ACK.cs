namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_USER_LOGINFAIL_ACK : NetworkMessage
    {
        public PROTO_NC_USER_LOGINFAIL_ACK(ushort error) : base(NetworkCommand.NC_USER_LOGINFAIL_ACK)
        {
            Write(error);
        }
    }
}
