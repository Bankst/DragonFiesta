namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DELETE_AVATAR : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DELETE_AVATAR(int charNo) : base(NetworkCommand.NC_LOG_GAME_DELETE_AVATAR)
		{
			Write(charNo);
		}
	}
}
