using DFEngine.Content.GameObjects.Movers;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_MOVER_MOVESPEED_CMD : NetworkMessage
	{
		public PROTO_NC_MOVER_MOVESPEED_CMD(MoverInstance mount) : base(NetworkCommand.NC_MOVER_MOVESPEED_CMD)
		{
			Write(mount.Handle);
			Write((ushort) mount.Stats.CurrentWalkSpeed);
			Write((ushort) mount.Stats.CurrentRunSpeed);
		}
	}
}
