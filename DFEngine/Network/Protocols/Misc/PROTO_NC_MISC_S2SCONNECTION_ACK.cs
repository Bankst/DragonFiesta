using System;

namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_MISC_S2SCONNECTION_ACK : NetworkMessage
    {
        public PROTO_NC_MISC_S2SCONNECTION_ACK(byte echoData, ushort error) : base(NetworkCommand.NC_MISC_S2SCONNECTION_ACK)
        {
			Write(echoData);
			Write(error);
        }
    }
}
