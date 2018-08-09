using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Maps.Interface;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public class SH08Handler
    {
        public static void SendBlockWalk(ZoneSession session, Position blockPosition)
        {
            SendBlockWalk(session, blockPosition.X, blockPosition.Y);
        }

        public static void SendBlockWalk(ZoneSession session, uint x, uint y)
        {
            using (var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_REINFORCE_STOP_CMD))
            {
                packet.Write<uint>(x);
                packet.Write<uint>(y);
                session.SendPacket(packet);
            }
        }

        public static void SendTeleport(ZoneSession session, Position teleportPosition)
        {
            SendTeleport(session, teleportPosition.X, teleportPosition.Y);
        }

        public static void SendTeleport(ZoneSession session, uint x, uint y)
        {
            using (var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_MOVEFAIL_CMD))
            {
                packet.Write<uint>(x);
                packet.Write<uint>(y);
                session.SendPacket(packet);
            }
        }

        public static FiestaPacket GetJumpPacket(IMapObject Object)
        {
            var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_SOMEONEJUMP_CMD);

            packet.Write<ushort>(Object.MapObjectId);
            return packet;
        }

        public static FiestaPacket GetStopPacket(IMapObject Object, Position stopPosition)
        {
            return GetStopPacket(Object, stopPosition.X, stopPosition.Y);
        }

        public static FiestaPacket GetStopPacket(IMapObject Object, uint stopX, uint stopY)
        {
            var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_SOMEONESTOP_CMD);

            packet.Write<ushort>(Object.MapObjectId);
            packet.Write<uint>(stopX);
            packet.Write<uint>(stopY);
            return packet;
        }

        public static FiestaPacket GetMovePacket(IMapObject Object, uint oldX, uint oldY, bool isRun, ushort speed = 115)
        {
            var packet = new FiestaPacket(Handler08Type._Header, (isRun ? Handler08Type.SMSG_ACT_SOMEONEMOVERUN_CMD : Handler08Type.SMSG_ACT_SOMEONEMOVEWALK_CMD));
            packet.Write<ushort>(Object.MapObjectId);
            packet.Write<uint>(oldX);
            packet.Write<uint>(oldY);
            packet.Write<uint>(Object.Position.X);
            packet.Write<uint>(Object.Position.Y);
            packet.Write<uint>(speed);
            return packet;
        }

        public static void SendChatMessage(ZoneSession session, ILivingObject Object, string message, bool isShout, ChatColor color = ChatColor.Normal)
        {
            //isShout = true;
            if (session.Character.Selection.SelectedObject == null) return;

            using (var packet = new FiestaPacket(Handler08Type._Header, (isShout ? Handler08Type.SMSG_ACT_SOMEONESHOUT_CMD : Handler08Type.SMSG_ACT_SOMEONECHAT_CMD)))
            {
                packet.Write<byte>(0); // ItemLinkDataCount
                packet.Write<ushort>(session.Character.Selection.SelectedObject.MapObjectId);
                packet.Write<byte>(message.Length);
                packet.Write<byte>(0); // GMColor ????
                packet.Write<byte>(0); // ChatWin ????
                packet.Write<byte>(isShout);
                packet.WriteString(message, message.Length);
                session.SendPacket(packet);
            }
        }

        public static void SendNotice(ZoneSession session, string message, byte unk = 0x65)
        {
            using (var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_NOTICE_CMD))
            {
                packet.Write<byte>(unk);//unk
                packet.Write<byte>(message.Length);
                packet.WriteString(message, message.Length);
                session.SendPacket(packet);
            }
        }
    }
}