using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.World.Game.Character;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    public static class SH05Handler
    {
        public static void SendCharacterCreationError(WorldSession Client, CharacterCreationError Error)
        {
            using (var packet = new FiestaPacket(Handler05Type._Header, Handler05Type.SMSG_AVATAR_CREATEFAIL_ACK))
            {
                packet.Write<ushort>((ushort)Error);
                Client.SendPacket(packet);
            }
        }

        public static void SendCharacterCreationOK(WorldSession Session, WorldCharacter Char)
        {
            using (var packet = new FiestaPacket(Handler05Type._Header, Handler05Type.SMSG_AVATAR_CREATESUCC_ACK))
            {
                packet.Write<byte>(1);//Slot?
                _SH04Helpers.WriteBasicInfo(Char, packet);
                Session.SendPacket(packet);
            }
        }

        public static void SendCharacterDeleteFail(WorldSession Session)
        {
            using (FiestaPacket mPacket = new FiestaPacket(Handler05Type._Header, Handler05Type.SMSG_AVATAR_ERASEFAIL_ACK))
            {
                mPacket.Write<ushort>(1);
                Session.SendPacket(mPacket);
            }
        }

        public static void SendCharacterDeleteComplete(WorldSession pSession, byte slot)
        {
            using (var Packet = new FiestaPacket(Handler05Type._Header, Handler05Type.SMSG_AVATAR_ERASESUCC_ACK))
            {
                Packet.Write<byte>(slot);
                pSession.SendPacket(Packet);
            }
        }

        public static void SendCharacterChangeName(WorldSession pSession, byte Slot, string NewName, bool IsOK)
        {
            using (var packet = new FiestaPacket(Handler05Type._Header, Handler05Type.SMSG_AVATAR_RENAME_ACK))
            {
                packet.Write<byte>(Slot);
                packet.WriteString(NewName, 20);
                packet.Write<uint>(0); //unk..
                packet.Write<ushort>(IsOK ? (ushort)208 : (ushort)209);
                pSession.SendPacket(packet);
            }
        }
    }
}