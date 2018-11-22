namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_USER_WILLLOGIN_ACK : NetworkMessage
    {
        public PROTO_NC_USER_WILLLOGIN_ACK(string guid, bool ok) : base(NetworkCommand.NC_USER_WILLLOGIN_ACK)
        {
            Write(guid, 32);
            Write(ok);
        }
    }
}
