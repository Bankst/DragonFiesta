namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_AVATAR_CREATESUCC_ACK : NetworkMessage
    {
        public PROTO_NC_AVATAR_CREATESUCC_ACK(byte characterCount, Content.GameObjects.Avatar avatar) : base(NetworkCommand.NC_AVATAR_CREATESUCC_ACK)
        {
            Write(characterCount);
            this.Write(avatar);
        }
    }
}
