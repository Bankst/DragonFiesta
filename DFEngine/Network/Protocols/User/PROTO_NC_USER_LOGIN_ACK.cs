using System.Collections.Generic;
using DFEngine.Server;

namespace DFEngine.Network.Protocols.User
{
	public class PROTO_NC_USER_LOGIN_ACK : NetworkMessage
    {
        public PROTO_NC_USER_LOGIN_ACK(List<World> worlds) : base(NetworkCommand.NC_USER_LOGIN_ACK)
        {
            Write((byte) worlds.Count);
            this.Write(worlds);
        }
    }
}
