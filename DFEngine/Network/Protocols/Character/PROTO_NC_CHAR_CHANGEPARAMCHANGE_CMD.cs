using System.Collections.Generic;
using DFEngine.Content.GameObjects.Characters;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CHANGEPARAMCHANGE_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CHANGEPARAMCHANGE_CMD(List<ParameterChange> changes) : base(NetworkCommand.NC_CHAR_CHANGEPARAMCHANGE_CMD)
		{
			Write((byte) changes.Count);
			foreach (var change in changes)
			{
				Write((byte) change.Flag);
				Write(change.Value);
			}
		}
	}
}
