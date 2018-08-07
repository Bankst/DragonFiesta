using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH20Handler
    {

        public static void SendUseHPStone(ZoneSession Session, bool Error = false)
        {
            using (var Packet = new FiestaPacket(Handler20Type._Header, Error ? Handler20Type.SMSG_SOULSTONE_USEFAIL_ACK : Handler20Type.SMSG_SOULSTONE_HP_USESUC_ACK))
            {
                Session.SendPacket(Packet);
            }
        }
        public static void SendUseSPStone(ZoneSession Session, bool Error = false)
        {
            using (var Packet = new FiestaPacket(Handler20Type._Header, Error ? Handler20Type.SMSG_SOULSTONE_USEFAIL_ACK : Handler20Type.SMSG_SOULSTONE_SP_USESUC_ACK))
            {
                Session.SendPacket(Packet);
            }
        }
        public static void SendUpdateHPStones(ZoneCharacter Character)
        {
            using (var Packet = new FiestaPacket(Handler20Type._Header, Handler20Type.SMSG_SOULSTONE_HP_BUY_ACK))
            {
                Packet.Write<ushort>(Character.Info.HPStones);
                Character.Session.SendPacket(Packet);
            }
        }
        public static void SendUpdateSPStones(ZoneCharacter Character)
        {
            using (var Packet = new FiestaPacket(Handler20Type._Header, Handler20Type.SMSG_SOULSTONE_SP_BUY_ACK))
            {
                Packet.Write<ushort>(Character.Info.SPStones);
                Character.Session.SendPacket(Packet);
            }
        }
    }
}
