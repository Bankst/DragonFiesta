namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK : NetworkMessage
    {
        public PROTO_NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK(byte xtrapServerKeyLength, byte[] xtrapServerKey) : base(NetworkCommand.NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK)
        {
            Write(xtrapServerKeyLength);
            Write(xtrapServerKey);
        }
    }
}
