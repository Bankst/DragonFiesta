using DFEngine.Network;

namespace IgniteEngine.Protocols
{
	public class PROTO_NC_MAP_LOGINFAIL_ACK : NetworkMessage
	{
		public PROTO_NC_MAP_LOGINFAIL_ACK(ushort error, byte fileIndex) : base(NetworkCommand.NC_MAP_LOGINFAIL_ACK)
		{
			Write(error);
			Write(fileIndex);
		}
	}
}
