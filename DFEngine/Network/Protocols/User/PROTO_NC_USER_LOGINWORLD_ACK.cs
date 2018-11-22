using System.Collections.Generic;

namespace DFEngine.Network.Protocols.User
{
    public class PROTO_NC_USER_LOGINWORLD_ACK : NetworkMessage
    {
        public PROTO_NC_USER_LOGINWORLD_ACK(ushort clientHandle, List<Content.GameObjects.Avatar> avatars) : base(NetworkCommand.NC_USER_LOGINWORLD_ACK)
        {
            Write(clientHandle);
            Write((byte) avatars.Count);
            this.Write(avatars);
        }
    }
}
