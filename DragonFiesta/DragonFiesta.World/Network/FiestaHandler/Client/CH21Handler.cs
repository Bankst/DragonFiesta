using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Utils.Config;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Friends;
using DragonFiesta.World.Network.FiestaHandler.Server;
using System.Linq;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler21Type._Header)]
    public static class CH21Handler
    {
		/*
        [PacketHandler(Handler21Type.CMSG_FRIEND_POINT_REQ)]
        public static void CMSG_FRIEND_POINT_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            SH21Handler.SendFrendPoints(sender.Character);
        }

        [PacketHandler(Handler21Type.CMSG_FRIEND_FIND_FRIENDS_REQ)]
        public static void CMSG_FRIEND_FIND_FRIENDS_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.IsCharacterLoggetIn())
            {
                sender.Dispose();
                return;
            }

            var DisplayList = WorldCharacterManager.Instance.OnlineCharacterList.Where(
                m => m.Session.Ingame && m.Info.Level >= sender.Character.Info.Level    //CheckLevek
                && m.Info.Name != sender.Character.Info.Name    //name
                && !sender.Character.Friends.Contains(m.Info.CharacterID)).OrderBy  //Check has already
                (m => m.Info.Level).Take(GameConfiguration.Instance.FriendSettings.MaxSearch);

            SH21Handler.SendFindFrendList(sender, DisplayList);

        }

        [PacketHandler(Handler21Type.CMSG_FRIEND_DEL_REQ)]
        public static void CMSG_FRIEND_DEL_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame
                || !packet.ReadString(out string SenderName, 20)
                || !packet.ReadString(out string ReciverName, 20))
            {
                sender.Dispose();
                return;
            }

            //get receiver character
            WorldCharacter receiver;
            if (!WorldCharacterManager.Instance.GetCharacterByName(ReciverName, out receiver, false))
            {
                SH21Handler.SendFriendDeleteResponse(sender, ReciverName, FriendDeleteResponse.CannotFindTarget);
                return;
            }

            FriendManager.RemoveFriend(sender.Character, receiver);
        }

        [PacketHandler(Handler21Type.CMSG_FRIEND_SET_CONFIRM_ACK)]
        public static void CMSG_FRIEND_SET_CONFIRM_ACK(WorldSession Session, FiestaPacket packet)
        {
            if (!Session.Ingame || !packet.ReadString(out string SenderName, 20) || !packet.ReadString(out string ReciverName, 20) || !packet.Read(out bool Accept))
            {
                Session.Dispose();
                return;
            }

            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByName(ReciverName, out WorldCharacter reciver, false))
                return;

            if (!Accept)
            {
                SH21Handler.SendFriendInviteRefuse(Session, ReciverName);
                return;
            }

            FriendManager.AddFriend(Session.Character, reciver, GameTime.Now().Time);
        }

        [PacketHandler(Handler21Type.CMSG_FRIEND_SET_REQ)]
        public static void CMSG_FRIEND_SET_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame
                || !packet.ReadString(out string SenderName, 20)
                || !packet.ReadString(out string ReciverName, 20))
            {
                sender.Dispose();
                return;
            }

            //Chek if want toa add self
            if (SenderName == ReciverName)
            {
                SH21Handler.SendFriendInviteResponse(sender, ReciverName, FriendInviteResponse.CannotAddSelf);
                return;
            }

            //Check max friend
            if (sender.Character.Friends.Count >= GameConfiguration.Instance.FriendSettings.MaxFriend)
            {
                SH21Handler.SendFriendInviteResponse(sender, ReciverName, FriendInviteResponse.HasMaxFriends);
                return;
            }

            //find Character
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByName(ReciverName, out WorldCharacter ReciverCharacter))
            {
                SH21Handler.SendFriendInviteResponse(sender, ReciverName, FriendInviteResponse.CannotFindTarget);
                return;
            }

            ///Hmmm is already frend?
            if (sender.Character.Friends.Contains(ReciverCharacter))
            {
                SH21Handler.SendFriendInviteResponse(sender, ReciverName, FriendInviteResponse.TargetIsAlreadyFriend);
                return;
            }
            //Check max friend
            if (ReciverCharacter.Friends.Count >= GameConfiguration.Instance.FriendSettings.MaxFriend)
            {
                SH21Handler.SendFriendInviteResponse(sender, ReciverName, FriendInviteResponse.TargetHasMaxFriends);
                return;
            }

            if (!ReciverCharacter.IsConnected)
            {
                SH21Handler.SendFriendInviteResponse(sender, ReciverName, FriendInviteResponse.CannotAddSelf);
                return;
            }

            SH21Handler.SendFriendInviteRequest(ReciverCharacter.Session, ReciverName, SenderName);
        }
		*/
    }
}
