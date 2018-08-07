using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler20Type._Header)]
    public static class CH20Handler
    {
        [PacketHandler(Handler20Type.CMSG_SOULSTONE_HP_USE_REQ)]
        public static void CMSG_SOULSTONE_HP_USE_REQ(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                SH04Helpers.SendZoneError(sender, ConnectionError.ClientDataError);
                sender.Dispose();
                return;
            }

            if (sender.Character.Info.HPStones < 1
                || sender.Character.LivingStats.HP >= sender.Character.Info.Stats.FullStats.MaxHP
                || !(sender.Character.AreaInfo.Map as LocalMap).Info.CanUseStone)
            {
                SH20Handler.SendUseHPStone(sender, true);
                return;
            }

            sender.Character.Info.HPStones--;
            sender.Character.LivingStats.HP += sender.Character.Info.LevelParameter.StoneHP;

            SH20Handler.SendUseHPStone(sender, false);
        }

        [PacketHandler(Handler20Type.CMSG_SOULSTONE_SP_USE_REQ)]
        public static void CMSG_SOULSTONE_SP_USE_REQ(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                SH04Helpers.SendZoneError(sender, ConnectionError.ClientDataError);
                sender.Dispose();
                return;
            }

            if(CharacterClass.ClassUsedLP(sender.Character.Info.Class))
            {
                return;
            }

            if (sender.Character.Info.SPStones < 1
                || sender.Character.LivingStats.SP >= sender.Character.Info.Stats.FullStats.MaxSP
                || !(sender.Character.AreaInfo.Map as LocalMap).Info.CanUseStone)
            {
                SH20Handler.SendUseSPStone(sender, true);
                return;
            }

            sender.Character.Info.SPStones--;
            sender.Character.LivingStats.SP += sender.Character.Info.LevelParameter.StoneSP;

            SH20Handler.SendUseSPStone(sender, false);
        }
    }
}
