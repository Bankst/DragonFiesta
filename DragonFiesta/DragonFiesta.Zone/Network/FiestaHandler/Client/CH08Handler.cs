using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Game.NPC;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using System;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler08Type._Header)]
    public static class CH08Handler
    {
        [PacketHandler(Handler08Type.CMSG_ACT_JUMP_CMD)]
        public static void CMSG_CHARACTER_JUMP(ZoneSession sender, FiestaPacket mpacket)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            if (sender.Character.State == CharacterState.Player ||
                sender.Character.State == CharacterState.OnMount)
            {
                using (var packet = SH08Handler.GetJumpPacket(sender.Character))
                {
                    sender.Character.Broadcast(packet, false);
                }
            }
        }

        [PacketHandler(Handler08Type.CMSG_ACT_MOVERUN_CMD)]
        public static void CMSG_CHARACTER_RUN(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            MoveCharacter(sender, packet, true, false);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_MOVEWALK_CMD)]
        public static void CMSG_CHARACTER_WALK(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            MoveCharacter(sender, packet, false, false);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_STOP_REQ)]
        public static void CMSG_CHARACTER_STOP(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            MoveCharacter(sender, packet, true, true);
        }

        private static void MoveCharacter(ZoneSession Session, FiestaPacket Packet, bool IsRun, bool IsStop)
        {
            if (Session.Character.State != CharacterState.Player
                && Session.Character.State != CharacterState.OnMount ||
                Session.IsLoggingOut)
                return;

            uint oldX,
                 oldY,

                 newX,
                 newY;

            if (IsStop)
            {
                if (!Packet.Read(out newX)
                    || !Packet.Read(out newY))
                {
                    Session.Dispose();
                    return;
                }

                oldX = Session.Character.Position.X;
                oldY = Session.Character.Position.Y;
            }
            else if (!Packet.Read(out oldX)
                     || !Packet.Read(out oldY)
                     || !Packet.Read(out newX)
                     || !Packet.Read(out newY))
            {
                Session.Dispose();
                return;
            }

            //check if character can move to this position
            if (!(Session.Character.Map as SectorMap).BlockInfos.CanWalk(newX, newY))
            {
                SH08Handler.SendBlockWalk(Session, newX, newY);
                SH08Handler.SendTeleport(Session, oldX, oldY);

                return;
            }

            //check the distance the character walked (possible hack if doesnt match)
            //todo: check also when character is on mount
            if (Session.Character.State == CharacterState.Player)
            {
                var distance = Position.GetDistance(newX, oldX, newY, oldY);

                if ((IsRun && distance > 500d)
                 || (!IsRun && distance > 400d))
                {
                    GameLog.Write(GameLogLevel.Warning, "Possible movement hack detected   >>   From: {0}:{1}   -   To: {2}:{3}   -   Distance: {4}", oldX, oldY, newX, newY, distance);
                    Session.Dispose();
                    return;
                }
            }

            //create new pos and move the character
            var newPos = new Position(newX, newY);

            if (!IsStop)
            {
                //get rotation based on old and new position
                uint deltaY = (newY - oldY),
                     deltaX = (newX - oldX);

                var radians = Math.Atan((double)deltaY / deltaX);
                var angle = (radians * (180d / Math.PI));

                newPos.Rotation = (byte)(angle / 2d);
            }
            else
            {
                newPos.Rotation = Session.Character.Position.Rotation;
            }

            Session.Character.Move(newPos, IsRun, IsStop);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_CHAT_REQ)]
        public static void CMSG_CHAT_MESSAGE_NORMAL(ZoneSession Session, FiestaPacket packet)
        {
            if (!Session.Ingame ||
                !packet.Read(out byte unk) ||
                !packet.Read(out byte Lenght) ||
                !packet.ReadEncodeString(out string Message, Lenght))

            {
                Session.Dispose();
                return;
            }

            (Session.Character.Map as LocalMap).MapChat.Chat(Session.Character, Message);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_SHOUT_CMD)]
        public static void CMSG_CHARACTER_SHOUT(ZoneSession Session, FiestaPacket packet)
        {
            if (!Session.Ingame ||
                !packet.Read(out byte Lenght) ||
                !packet.ReadEncodeString(out string Message, Lenght))

            {
                Session.Dispose();
                return;
            }

            (Session.Character.Map as LocalMap).MapShout.Chat(Session.Character, Message);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_NPCCLICK_CMD)]
        public static void CMSG_CHARACTER_BEGIN_INTERACTION(ZoneSession Session, FiestaPacket packet)
        {
            if (!Session.Ingame ||
                !packet.Read(out ushort MapObjectId))
            {
                Session.Dispose();
                return;
            }

            if ((Session.Character.Map as LocalMap).GetObjectByID(MapObjectId, out IMapObject obj)
                && obj.Type == MapObjectType.NPC)
            {
                (obj as NPCBase).HandleInteraction(Session.Character);
            }
        }
    }
}