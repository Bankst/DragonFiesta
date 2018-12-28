using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_ACT_SOMEONEMOVERUN_CMD : NetworkMessage
	{
		public PROTO_NC_ACT_SOMEONEMOVERUN_CMD(GameObject obj, Vector2 from, Vector2 to) : base(NetworkCommand.NC_ACT_SOMEONEMOVERUN_CMD)
		{
			Write(obj.Handle);
			Write((uint) from.X);
			Write((uint) from.Y);
			Write((uint) to.X);
			Write((uint) to.Y);
			Write((ushort) obj.Stats.CurrentRunSpeed);
			Write((ushort) 0);
		}
	}
}
