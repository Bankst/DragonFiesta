using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Friends;
using System.Collections.Generic;
using System.Linq;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    public static class SH21Handler
    {
		/*
        public static void WriteFriendInfo(WorldCharacter myFriend, FiestaPacket packet)
        {
            packet.Write<byte>(myFriend.LoginInfo.IsOnline ? 0x01 : 0x02);
            packet.Write<byte>(myFriend.LoginInfo.LastLogin.Month << 4);
            packet.Write<byte>(myFriend.LoginInfo.LastLogin.Day);
            packet.Write<byte>(15);
            packet.WriteString(myFriend.Info.Name, 20);
            packet.Write<byte>(myFriend.Info.Class);
            packet.Write<byte>(myFriend.Info.Level);
            packet.Write<bool>(0);  //IsInGroup
            packet.Write<byte>(1);  //flag
            packet.WriteString(myFriend.AreaInfo.MapInfo.Index, 12);
            packet.Fill(32, 0x00);  //statustitel
        }

        public static void SendFrendPointDiff(WorldSession session, ushort givedPoints)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_GET_DIFF_FRIEND_POINT_CMD))
            {
                pack.Write<ushort>(givedPoints);
                session.SendPacket(pack);
            }
        }

        public static void SendFindFrendList(WorldSession session, IEnumerable<WorldCharacter> list)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_FIND_FRIENDS_ACK))
            {
                pack.Write<ushort>(14016);  //Succes Error code
                pack.Write<ushort>(list.Count());

                foreach (var Char in list)
                {
                    WriteFriendInfo(Char, pack);
                }
                session.SendPacket(pack);
            }
        }

        public static void SendFriendList(WorldCharacter character)
        {

            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LIST_CMD))
            {
                pack.Write<byte>(character.Friends.Count);

                for (int i = 0; i < character.Friends.Count; i++)
                {
                    WriteFriendInfo(character.Friends[i].MyFriend, pack);
                }
                character.Session.SendPacket(pack);
            }
        }

        public static void BroadcastFriendLoggedOut(WorldCharacter character) =>

            character.Friends.FriendAction(friend =>
            {
                SendFriendLoggedOut(friend.MyFriend.Session, character);
            });

        public static void BroadcastFriendLoggedIn(WorldCharacter character) =>

            character.Friends.FriendAction(frend =>
            {
                SendFriendLoggedIn(frend.MyFriend.Session, character);
            });

        public static void SendFriendLoggedIn(WorldSession session, WorldCharacter character)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LOGIN_CMD))
            {
                pack.WriteString(character.Info.Name, 20);
                pack.WriteString(character.AreaInfo.MapInfo.Index, 12);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendLoggedOut(WorldSession session,WorldCharacter character)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LOGOUT_CMD))
            {
                pack.WriteString(character.Info.Name, 20);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendUpdateLevel(WorldCharacter character, byte newLevel)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LEVEL_CMD))
            {
                pack.WriteString(character.Info.Name, 20);
                pack.Write<byte>(newLevel);
                character.Friends.Broadcast(pack);
            }
        }

        public static void SendFriendUpdateMap(WorldCharacter character, MapInfo newMap)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_MAP_CMD))
            {
                pack.WriteString(character.Info.Name, 20);
                pack.WriteString(newMap.Index, 12);
                character.Friends.Broadcast(pack);
            }
        }

        public static void SendFrendChangeClass(WorldCharacter character,ClassId newClass)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_CLASS_CHANGE_CMD))
            {
                pack.WriteString(character.Info.Name, 20);
                pack.Write<byte>(character.Info.Class);
                character.Friends.Broadcast(pack);
            }
        }

        public static void SendFriendInviteRefuse(WorldSession session, string reciverName)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_REFUSE_CMD))
            {
                pack.WriteString(reciverName, 20);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendInviteRequest(WorldSession session, string reciverName, string senderName)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_SET_CONFIRM_REQ))
            {
                pack.WriteString(reciverName, 20);
                pack.WriteString(senderName, 20);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendInviteResponse(WorldSession session, string reciverName, FriendInviteResponse response)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_SET_ACK))
            {
                pack.WriteString(session.Character.Info.Name, 20);
                pack.WriteString(reciverName, 20);
                pack.Write<ushort>(response);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendExtraInfo(WorldSession session, WorldCharacter frend)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_ADD_CMD))
            {
                WriteFriendInfo(frend, pack);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendDeleteResponse(WorldSession session, string reciverName, FriendDeleteResponse response)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_DEL_ACK))
            {
                pack.WriteString(session.Character.Info.Name, 20);
                pack.WriteString(reciverName, 20);
                pack.Write<ushort>(response);
                session.SendPacket(pack);
            }
        }

        public static void SendFriendDeletedYou(WorldSession session, string senderName)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_DEL_CMD))
            {
                pack.WriteString(senderName, 20);
                session.SendPacket(pack);
            }
        }

        public static void SendFrendPoints(WorldCharacter character)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_POINT_ACK))
            {
                pack.Write<ushort>(character.Info.FriendPoints);
                character.Session.SendPacket(pack);
            }
        }
	*/
	}
}
