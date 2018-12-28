using DFEngine.Content.Game;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_ACT_REINFORCE_STOP_CMD : NetworkMessage
	{
		public PROTO_NC_ACT_REINFORCE_STOP_CMD(Vector2 location) : base(NetworkCommand.NC_ACT_REINFORCE_STOP_CMD)
		{
			Write((uint) location.X);
			Write((uint) location.Y);
		}
	}
}
