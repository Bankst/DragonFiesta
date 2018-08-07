using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Data.Friends;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH21Handler
    {
        public static void SendFriendPointUse(ZoneSession Session,
            FriendPointReward Reward)
        {
            //TMP
            using (var Packet = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_UES_FRIEND_POINT_ACK))
            {
                Packet.Write<ushort>(14016);//ErrorCode
                Packet.Write<ushort>(Session.Character.Info.FriendPoints);//Rest Frendpoint
                Packet.Write<ushort>(Reward.ItemId);//itemId
                Packet.Write<byte>(Reward.Amount);//amount
                Session.SendPacket(Packet);
            }
        }
    }
}
