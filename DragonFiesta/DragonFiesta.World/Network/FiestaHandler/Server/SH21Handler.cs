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
    
        public static void WriteFriendInfo(WorldCharacter MyFriend, FiestaPacket Packet)
        {
            Packet.Write<byte>(MyFriend.LoginInfo.IsOnline ? 0x01 : 0x02);
            Packet.Write<byte>(MyFriend.LoginInfo.LastLogin.Month << 4);
            Packet.Write<byte>(MyFriend.LoginInfo.LastLogin.Day);
            Packet.Write<byte>(15);
            Packet.WriteString(MyFriend.Info.Name, 20);
            Packet.Write<byte>(MyFriend.Info.Class);
            Packet.Write<byte>(MyFriend.Info.Level);
            Packet.Write<bool>(0);  //IsInGroup
            Packet.Write<byte>(1);  //flag
            Packet.WriteString(MyFriend.AreaInfo.MapInfo.Index, 12);
            Packet.Fill(32, 0x00);  //statustitel
        }

        public static void SendFrendPointDiff(WorldSession Session, ushort GivedPoints)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_GET_DIFF_FRIEND_POINT_CMD))
            {
                pack.Write<ushort>(GivedPoints);
                Session.SendPacket(pack);
            }
        }

        public static void SendFindFrendList(WorldSession Session, IEnumerable<WorldCharacter> List)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_FIND_FRIENDS_ACK))
            {
                pack.Write<ushort>(14016);  //Succes Error code
                pack.Write<ushort>(List.Count());

                foreach (var Char in List)
                {
                    WriteFriendInfo(Char, pack);
                }
                Session.SendPacket(pack);
            }
        }

        public static void SendFriendList(WorldCharacter Character)
        {

            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LIST_CMD))
            {
                pack.Write<byte>(Character.Friends.Count);

                for (int i = 0; i < Character.Friends.Count; i++)
                {
                    WriteFriendInfo(Character.Friends[i].MyFriend, pack);
                }
                Character.Session.SendPacket(pack);
            }
        }

        public static void BroadcastFriendLoggedOut(WorldCharacter Character) =>

            Character.Friends.FriendAction((frend) =>
            {
                SendFriendLoggedOut(frend.MyFriend.Session, Character);
            }, true);

        public static void BroadcastFriendLoggedIn(WorldCharacter Character) =>

            Character.Friends.FriendAction((frend) =>
            {
                SendFriendLoggedIn(frend.MyFriend.Session, Character);
            }, true);


        public static void SendFriendLoggedIn(WorldSession Session, WorldCharacter Character)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LOGIN_CMD))
            {
                Pack.WriteString(Character.Info.Name, 20);
                Pack.WriteString(Character.AreaInfo.MapInfo.Index, 12);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFriendLoggedOut(WorldSession Session,WorldCharacter Character)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LOGOUT_CMD))
            {
                Pack.WriteString(Character.Info.Name, 20);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFriendUpdateLevel(WorldCharacter Character, byte NewLevel)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_LEVEL_CMD))
            {
                Pack.WriteString(Character.Info.Name, 20);
                Pack.Write<byte>(NewLevel);
                Character.Friends.Broadcast(Pack);
            }
        }

        public static void SendFriendUpdateMap(WorldCharacter Character, MapInfo NewMap)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_MAP_CMD))
            {
                Pack.WriteString(Character.Info.Name, 20);
                Pack.WriteString(NewMap.Index, 12);
                Character.Friends.Broadcast(Pack);
            }
        }

        public static void SendFrendChangeClass(WorldCharacter Character,ClassId NewClass)
        {
            using (var pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_CLASS_CHANGE_CMD))
            {
                pack.WriteString(Character.Info.Name, 20);
                pack.Write<byte>(Character.Info.Class);
                Character.Friends.Broadcast(pack);
            }
        }

        public static void SendFriendInviteRefuse(WorldSession Session, string ReciverName)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_REFUSE_CMD))
            {
                Pack.WriteString(ReciverName, 20);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFriendInviteRequest(WorldSession Session, string ReciverName, string SenderName)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_SET_CONFIRM_REQ))
            {
                Pack.WriteString(ReciverName, 20);
                Pack.WriteString(SenderName, 20);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFriendInviteResponse(WorldSession session, string ReciverName, FriendInviteResponse response)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_SET_ACK))
            {
                Pack.WriteString(session.Character.Info.Name, 20);
                Pack.WriteString(ReciverName, 20);
                Pack.Write<ushort>(response);
                session.SendPacket(Pack);
            }
        }

        public static void SendFriendExtraInfo(WorldSession Session, WorldCharacter Frend)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_ADD_CMD))
            {
                WriteFriendInfo(Frend, Pack);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFriendDeleteResponse(WorldSession Session, string ReciverName, FriendDeleteResponse response)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_DEL_ACK))
            {
                Pack.WriteString(Session.Character.Info.Name, 20);
                Pack.WriteString(ReciverName, 20);
                Pack.Write<ushort>(response);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFriendDeletedYou(WorldSession Session, string SenderName)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_DEL_CMD))
            {
                Pack.WriteString(SenderName, 20);
                Session.SendPacket(Pack);
            }
        }

        public static void SendFrendPoints(WorldCharacter Character)
        {
            using (var Pack = new FiestaPacket(Handler21Type._Header, Handler21Type.SMSG_FRIEND_POINT_ACK))
            {
                Pack.Write<ushort>(Character.Info.FriendPoints);
                Character.Session.SendPacket(Pack);
            }
        }
    }
}
