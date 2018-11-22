namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_MISC_S2SCONNECTION_REQ : NetworkMessage
    {
        public PROTO_NC_MISC_S2SCONNECTION_REQ(NetworkConnectionType from, NetworkConnectionType to, byte worldNo, string worldName, byte zoneNo, string ip, ushort port) : base(NetworkCommand.NC_MISC_S2SCONNECTION_REQ)
        {
            Write((byte) from);
            Write((byte) to);
            Write(worldNo);
            Write(worldName, 20);
            Write(zoneNo);
            Write(ip, 16);
            Write(port);
            Write((ushort) ((byte) from + (byte) to));
        }
    }
}
