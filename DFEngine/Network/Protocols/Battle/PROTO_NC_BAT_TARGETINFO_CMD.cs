using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_BAT_TARGETINFO_CMD : NetworkMessage
	{
		public PROTO_NC_BAT_TARGETINFO_CMD(byte order, GameObject target) : base(NetworkCommand.NC_BAT_TARGETINFO_CMD)
		{
			Write(order);
			Write(target?.Handle ?? ushort.MaxValue);
			if (target?.Stats == null)
				return;
			Write(target.Stats.CurrentHP);
			Write(target.Stats.CurrentMaxHP);
			Write(target.Stats.CurrentSP);
			Write(target.Stats.CurrentMaxSP);
			Write(target.Stats.CurrentLP);
			Write(target.Stats.CurrentMaxLP);
			Write(target.Level);
			Write(target.HPChangeOrder);
		}
	}
}
