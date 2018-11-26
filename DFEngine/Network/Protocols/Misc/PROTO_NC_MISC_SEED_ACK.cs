namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_MISC_SEED_ACK : NetworkMessage
    {
        public PROTO_NC_MISC_SEED_ACK(ushort seed) : base(NetworkCommand.NC_MISC_SEED_ACK)
        {
            Write(seed);
        }
    }
}
