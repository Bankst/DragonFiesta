namespace DFEngine.Network.Protocols.Log
{
    public class PROTO_NC_LOG_GAME_CREATE_AVATAR : NetworkMessage
    {
		public PROTO_NC_LOG_GAME_CREATE_AVATAR(int charNo) : base(NetworkCommand.NC_LOG_GAME_CREATE_AVATAR)
		{
			Write(charNo);
		}
    }
}
