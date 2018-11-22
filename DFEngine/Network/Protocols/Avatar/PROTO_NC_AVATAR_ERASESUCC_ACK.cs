namespace DFEngine.Network.Protocols.Avatar
{
    public class PROTO_NC_AVATAR_ERASESUCC_ACK : NetworkMessage
    {
        public PROTO_NC_AVATAR_ERASESUCC_ACK(byte slot) : base(NetworkCommand.NC_AVATAR_ERASESUCC_ACK)
        {
            Write(slot);
        }
    }
}
