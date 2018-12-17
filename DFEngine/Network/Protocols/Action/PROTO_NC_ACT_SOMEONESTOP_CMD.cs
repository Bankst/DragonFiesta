using System.Runtime.Serialization;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_ACT_SOMEONESTOP_CMD : NetworkMessage
	{
		public PROTO_NC_ACT_SOMEONESTOP_CMD(GameObject obj, Vector2 location) : base(NetworkCommand.NC_ACT_SOMEONESTOP_CMD)
		{
			Write(obj.Handle);
			Write((uint) location.X);
			Write((uint) location.Y);
		}
	}
}
