namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_USER_WILLLOGIN_REQ : NetworkMessage
    {
        public PROTO_NC_USER_WILLLOGIN_REQ(Account account, string guid) : base(NetworkCommand.NC_USER_WILLLOGIN_REQ)
        {
            Write(account.UserNo);
            Write(account.Username, 256);
            Write(guid, 32);
        }
    }
}
