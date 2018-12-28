using DFEngine.Network;

namespace WorldManagerServer.Handlers
{
	public static class PrisonHandlers
	{
		public static void NC_PRISON_GET_REQ(NetworkMessage message, NetworkConnection client)
		{
			new PROTO_NC_PRISON_GET_ACK(0xB10D).Send(client);
		}
	}
}
