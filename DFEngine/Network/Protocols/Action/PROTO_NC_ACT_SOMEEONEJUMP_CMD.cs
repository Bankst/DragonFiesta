using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_ACT_SOMEEONEJUMP_CMD : NetworkMessage
	{
		public PROTO_NC_ACT_SOMEEONEJUMP_CMD(GameObject obj) : base(NetworkCommand.NC_ACT_SOMEEONEJUMP_CMD)
		{
			Write(obj.Handle);
		}
	}
}
