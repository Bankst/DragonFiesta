using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.NPC;

namespace DragonFiesta.Zone.Network.Helpers
{
    public class SH08Helper
    {

        public static FiestaPacket GetNPCInterActionPacket(NPCBase NPC)
        {
            var packet = new FiestaPacket(Handler08Type._Header, Handler08Type.SMSG_ACT_NPCMENUOPEN_REQ);

            packet.Write<ushort>(NPC.Info.MobInfo.ID);

            return packet;
        }
    }
}
