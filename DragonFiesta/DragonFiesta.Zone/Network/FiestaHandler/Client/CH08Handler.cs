using DragonFiesta.Networking.HandlerTypes;
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

	    [PacketHandler(Handler08Type.CMSG_ACT_NPCMENUOPEN_ACK)]
	    public static void CMSG_CHARACTER_OPENMENU_ACK(ZoneSession sender, FiestaPacket packet)
	    {
		    var selection = (NPCBase) sender.Character.Selection.SelectedObject;
			selection.OpenMenu(sender.Character);
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

        private static void MoveCharacter(ZoneSession session, FiestaPacket packet, bool isRun, bool isStop)
        {
            if (session.Character.State != CharacterState.Player
                && session.Character.State != CharacterState.OnMount ||
                session.IsLoggingOut)
                return;

            uint oldX,
                 oldY,

                 newX,
                 newY;

            if (isStop)
            {
                if (!packet.Read(out newX)
                    || !packet.Read(out newY))
                {
                    session.Dispose();
                    return;
                }

                oldX = session.Character.Position.X;
                oldY = session.Character.Position.Y;
            }
            else if (!packet.Read(out oldX)
                     || !packet.Read(out oldY)
                     || !packet.Read(out newX)
                     || !packet.Read(out newY))
            {
                session.Dispose();
                return;
            }

            //check if character can move to this position
            if (!((SectorMap) session.Character.Map).BlockInfos.CanWalk(newX, newY))
            {
                SH08Handler.SendBlockWalk(session, newX, newY);
                SH08Handler.SendTeleport(session, oldX, oldY);

                return;
            }

            //check the distance the character walked (possible hack if doesnt match)
            //todo: check also when character is on mount
            if (session.Character.State == CharacterState.Player)
            {
                var distance = Position.GetDistance(newX, oldX, newY, oldY);

                if (isRun && distance > 500d
                 || !isRun && distance > 400d)
                {
                    GameLog.Write(GameLogLevel.Warning, "Possible movement hack detected >> From: {0}:{1} - To: {2}:{3} - Distance: {4}", oldX, oldY, newX, newY, distance);
                    session.Dispose();
                    return;
                }
            }

            //create new pos and move the character
            var newPos = new Position(newX, newY);

            if (!isStop)
            {
                //get rotation based on old and new position
                uint deltaY = (newY - oldY),
                     deltaX = (newX - oldX);

                var radians = Math.Atan((double)deltaY / deltaX);
                var angle = radians * (180d / Math.PI);

                newPos.Rotation = (byte)(angle / 2d);
            }
            else
            {
                newPos.Rotation = session.Character.Position.Rotation;
            }

            session.Character.Move(newPos, isRun, isStop);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_CHAT_REQ)]
        public static void CMSG_CHAT_MESSAGE_NORMAL(ZoneSession session, FiestaPacket packet)
        {
            if (!session.Ingame ||
                !packet.Read(out byte unk) ||
                !packet.Read(out byte length) ||
                !packet.ReadEncodeString(out var message, length))

            {
                session.Dispose();
                return;
            }

            ((LocalMap) session.Character.Map).MapChat.Chat(session.Character, message);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_SHOUT_CMD)]
        public static void CMSG_CHARACTER_SHOUT(ZoneSession session, FiestaPacket packet)
        {
            if (!session.Ingame ||
                !packet.Read(out byte length) ||
                !packet.ReadEncodeString(out var message, length))

            {
                session.Dispose();
                return;
            }

            ((LocalMap) session.Character.Map).MapShout.Chat(session.Character, message);
        }

        [PacketHandler(Handler08Type.CMSG_ACT_NPCCLICK_CMD)]
        public static void CMSG_CHARACTER_BEGIN_INTERACTION(ZoneSession session, FiestaPacket packet)
        {
            if (!session.Ingame ||
                !packet.Read(out ushort mapObjectId))
            {
                session.Dispose();
                return;
            }

            if (((LocalMap) session.Character.Map).GetObjectByID(mapObjectId, out var obj)
                && obj.Type == MapObjectType.NPC)
            {
                ((NPCBase) obj).HandleInteraction(session.Character);
            }
        }
    }
}