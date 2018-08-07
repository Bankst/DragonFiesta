using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH09Handler
    {
        public static void SendSelectionUpdate(ZoneSession Reciver, ILivingObject SelectedObject, bool IsSelectedBy)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_TARGETINFO_CMD))
            {
                packet.Write<bool>(IsSelectedBy);
                packet.Write<ushort>(SelectedObject.MapObjectId);
                // (pdb) after mapobjectid 6 x uint for HP/SP and LP check this..
                SelectedObject.WriteSelectionUpdate(packet);
                Reciver.SendPacket(packet);
            }
        }

        public static void SendGainEXP(ZoneCharacter Character, uint Amount, ushort MobID = 0xFFFF)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_EXPGAIN_CMD))
            {
                packet.Write<uint>(Amount);
                packet.Write<ushort>(MobID);
                Character.Session.SendPacket(packet);
            }
        }

        public static void SendLevelUpAnimation(ZoneCharacter Character, ushort MobID = 0xFFFF)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_SOMEONELEVELUP_CMD))
            {
                packet.Write<ushort>(Character.MapObjectId);
                packet.Write<ushort>(MobID);
                Character.Broadcast(packet, true);
            }
        }

        public static void SendLevelUpData(ZoneCharacter Character, ushort MobID = 0xFFFF)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_LEVELUP_CMD))
            {
                packet.Write<byte>(Character.Info.Level);
                packet.Write<ushort>(MobID);
                SH06Helper.WriteDetailedInfoExtra(Character, packet, true);
                Character.Session.SendPacket(packet);
            }
        }

        public static void SendLPUpdate(ZoneCharacter Character)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_LPCHANGE_CMD))
            {
                packet.Write<uint>(Character.LivingStats.LP);
                packet.Write<ushort>(Character.UpdateCounter);
                Character.Session.SendPacket(packet);
            }
        }

        public static void SendHPUpdate(ZoneCharacter Character)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_HPCHANGE_CMD))
            {
                packet.Write<uint>(Character.LivingStats.HP);
                packet.Write<ushort>(Character.UpdateCounter);
                Character.Session.SendPacket(packet);
            }
        }

        public static void SendSPUpdate(ZoneCharacter Character)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_SPCHANGE_CMD))
            {
                packet.Write<uint>(Character.LivingStats.SP);
                packet.Write<ushort>(Character.UpdateCounter);
                Character.Session.SendPacket(packet);
            }
        }

        public static void CeaseFire(ZoneCharacter character, ILivingObject livingObject)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_CEASE_FIRE_CMD))
            {
                packet.Write<ushort>(livingObject.MapObjectId);
                character.Session.SendPacket(packet);
            }
        }

        public static void DotDamage(ZoneCharacter character, ILivingObject livingObject)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_DOTDAMAGE_CMD))
            {
                packet.Write<ushort>(livingObject.MapObjectId);
                packet.Write<uint>(0); // Resthp
                packet.Write<ushort>(0); // Damage
                packet.Write<ushort>(0); // Abstate
                packet.Write<ushort>(0); // HPChangeOrder
                packet.Write<byte>(0); // IsMissDamage
                character.Session.SendPacket(packet);
            }
        }

        public static void SendAbstateRest(ZoneCharacter character, ILivingObject livingObject)
        {
            using (var packet = new FiestaPacket(Handler09Type._Header, Handler09Type.SMSG_BAT_ABSTATERESET_CMD))
            {
                packet.Write<ushort>(livingObject.MapObjectId);
                packet.Write<uint>(0); // AbstateIndex
                character.Session.SendPacket(packet);
            }
        }
    }
}