using DFEngine.Server;

namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_USER_WORLDSELECT_ACK : NetworkMessage
    {
        public PROTO_NC_USER_WORLDSELECT_ACK(World world, string guid) : base(NetworkCommand.NC_USER_WORLDSELECT_ACK)
        {
            Write(world.Status);
            Write(world.IP, 16);
            Write(world.Port);
            Write(guid, 32);
        }
    }
}
