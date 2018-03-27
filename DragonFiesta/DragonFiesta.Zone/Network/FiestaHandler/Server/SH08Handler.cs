using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Maps.Interface;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public class SH08Handler
    {
        public static void SendBlockWalk(ZoneSession Session, Position BlockPosition)
        {
            SendBlockWalk(Session, BlockPosition.X, BlockPosition.Y);
        }

        public static void SendBlockWalk(ZoneSession Session, uint X, uint Y)
        {
            using (var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_REINFORCE_STOP_CMD))
            {
                packet.Write<uint>(X);
                packet.Write<uint>(Y);
                Session.SendPacket(packet);
            }
        }

        public static void SendTeleport(ZoneSession Session, Position TeleportPosition)
        {
            SendTeleport(Session, TeleportPosition.X, TeleportPosition.Y);
        }

        public static void SendTeleport(ZoneSession Session, uint X, uint Y)
        {
            using (var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_MOVEFAIL_CMD))
            {
                packet.Write<uint>(X);
                packet.Write<uint>(Y);
                Session.SendPacket(packet);
            }
        }

        public static FiestaPacket GetJumpPacket(IMapObject Object)
        {
            var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_SOMEEONEJUMP_CMD);

            packet.Write<ushort>(Object.MapObjectId);
            return packet;
        }

        public static FiestaPacket GetStopPacket(IMapObject Object, Position StopPosition)
        {
            return GetStopPacket(Object, StopPosition.X, StopPosition.Y);
        }

        public static FiestaPacket GetStopPacket(IMapObject Object, uint StopX, uint StopY)
        {
            var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_SOMEONESTOP_CMD);

            packet.Write<ushort>(Object.MapObjectId);
            packet.Write<uint>(StopX);
            packet.Write<uint>(StopY);
            return packet;
        }

        public static FiestaPacket GetMovePacket(IMapObject Object, uint OldX, uint OldY, bool IsRun, ushort Speed = 115)
        {
            var packet = new FiestaPacket(Handler08Type._Header, (IsRun ? Handler08Type.SMSG_ACT_SOMEONEMOVERUN_CMD : Handler08Type.SMSG_ACT_SOMEONEMOVEWALK_CMD));
            packet.Write<ushort>(Object.MapObjectId);
            packet.Write<uint>(OldX);
            packet.Write<uint>(OldY);
            packet.Write<uint>(Object.Position.X);
            packet.Write<uint>(Object.Position.Y);
            packet.Write<uint>(Speed);
            return packet;
        }

        public static void SendChatMessage(ZoneSession Session, ILivingObject Object, string Message, bool IsShout, ChatColor Color = ChatColor.Normal)
        {
            IsShout = true;
            if (Session.Character.Selection.SelectedObject == null) return;

            using (var Packet = new FiestaPacket(Handler08Type._Header, (IsShout ? Handler08Type.SMSG_ACT_SOMEONESHOUT_CMD : Handler08Type.SMSG_ACT_SOMEONECHAT_CMD)))
            {
                Packet.Write<ushort>(Session.Character.Selection.SelectedObject.MapObjectId);
                Packet.WriteHexAsBytes("04 1C 00 00 00 48 61 68 61 2C 20 64 75 20 6B 72 69 65 67 73 74 20 6D 69 63 68 20 6E 69 63 68 74 21");

                /* new

                    // Normal Chat Packet ohne Item link
                    Packet.Write<byte>(0);  //unk
                    Packet.Write<ushort>(Object.MapObjectId);
                    Packet.Write<byte>(Message.Length);
                    Packet.Write<byte>(0);  //unk 
                    Packet.Write<byte>(0);  //unk
                    Packet.Write<byte>(0);  //unk
                    Packet.WriteString(Message, Message.Length);

                    // Item link Struc für Normal Chat -> nutzt selbe Packet wie Normal Chat aber andere Struc
                    Packet.Write<byte>(0);  //unk
                    Packet.Write<ushort>(Object.MapObjectId);
                    Packet.Write<byte>(Message.Length);
                    Packet.Write<byte>(0);  //unk 
                    Packet.Write<byte>(0);  //unk
                    Packet.Write<byte>(0);  //unk
                    Packet.WriteString(Message, Message.Length); // z.b. {color,lightblue,<	Eremitenstab [+10]	>}
                    Packet.Write<ushort>(ItemID);   // Id des Items
                    Packet.Write<byte>(Up-Wert); // gibt an ob Waffe/Rüssi +1 bis +13 ist
                    Packet.Fill(26,0x00);   //unk
                    Packet.WriteString(xxxx, 20)    // Charackter Name der die Lizenz auf die Waffe gemacht hat
                    Packet.Fill(54,0x00);   //unk

                    // Shout Packet ohne Item Link
                    Packet.Write<byte>(0);  //unk
                    Packet.WriteString((Object as ZoneCharacter).Info.Name, 20);
                    Packet.Write<byte>(0);  //unk - Color ?
                    Packet.Write<byte>(Message.Length);
                    Packet.WriteString(Message, Message.Length);

                    // Shout Packet mit Item link -> nutzt das Shout Packet, aber andere Struc
                    Packet.Write<byte>(0);  //unk
                    Packet.WriteString((Object as ZoneCharacter).Info.Name, 20);
                    Packet.Write<byte>(0);  //unk - Color ?
                    Packet.Write<byte>(Message.Length);
                    Packet.WriteString(Message, Message.Length);
                    Packet.Write<ushort>(ItemID);   // Id des Items
                    Packet.Write<byte>(Up-Wert); // gibt an ob Waffe/Rüssi +1 bis +13 ist
                    Packet.Fill(26,0x00);   //unk
                    Packet.WriteString(xxxx, 20)    // Charackter Name der die Lizenz auf die Waffe gemacht hat
                    Packet.Fill(54,0x00);   //unk

                */


                // old
                // Packet.WriteHexAsBytes("00 F3 03 17 00 00 00 49 63 68 20 62 69 6E 20 7A 69 65 6D 6C 69 63 68 20 73 74 61 72 6B 21");

                //  Packet.Write<byte>(0x03);

                //       Packet.Write<byte>(0x03);
                //    Packet.Write<byte>(Message.Length);
                //Packet.Fill(3, 0x00);//unk
                //  Packet.WriteString(Message, Message.Length);
                /*
                if (!IsShout)
                {
                    Packet.Write<ushort>(Object.MapObjectId);
                    Packet.Write<byte>(0x03);//unk
                    Packet.Write<byte>(Message.Length);
                    Packet.Fill(3,0x00);//unk
                }
                else
                {
                    Packet.WriteString((Object as ZoneCharacter).Info.Name, 20);
                    Packet.Write<byte>(Color);
                    Packet.Write<byte>(Message.Length);
                }
                Packet.WriteString(Message, Message.Length);*/
                Session.SendPacket(Packet);
            }
        }

        public static void SendNotice(ZoneSession Session, string Message, byte unk = 0x65)
        {
            using (var Packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_NOTICE_CMD))
            {
                Packet.Write<byte>(unk);//unk
                Packet.Write<byte>(Message.Length);
                Packet.WriteString(Message, Message.Length);
                Session.SendPacket(Packet);
            }
        }
    }
}