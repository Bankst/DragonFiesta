using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH18Handler
    {
        public static void SendRemainingSkillPoints(ZoneCharacter Character)
        {
            using (var Packet = new FiestaPacket(Handler18Type._Header, Handler18Type.SMSG_SKILL_EMPOWPOINT_CMD))
            {
                Packet.Write<byte>(Character.Info.SkillPoints);
                Character.Session.SendPacket(Packet);
            }
        }
        public static void SendSkillLearned(ZoneCharacter Character, ushort SkillID)
        {
            using (var Packet = new FiestaPacket(Handler18Type._Header, Handler18Type.SMSG_SKILL_SKILL_LEARNSUC_CMD))
            {
                Packet.Write<ushort>(SkillID);
                Packet.Write<byte>(0); // hm ?
                Character.Session.SendPacket(Packet);
            }
        }
    }
}
