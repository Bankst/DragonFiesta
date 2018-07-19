using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Network.Helpers
{
    public static class SH06Helper
    {
        public static void WriteDetailedInfoExtra(ZoneCharacter Character, FiestaPacket Packet, bool IsLevelUp = false)
        {
            /*
            struct SHINE_CHAR_STATVAR
            {
              unsigned int base;
              unsigned int change;
            };

            struct CHAR_PARAMETER_DATA::<unnamed-type-PwrStone>
            {
              unsigned int flag;
              unsigned int EPPysic;
              unsigned int EPMagic;
              unsigned int MaxStone;
            };


            struct CHAR_PARAMETER_DATA
            {
              unsigned __int64 PrevExp;
              unsigned __int64 NextExp;
              SHINE_CHAR_STATVAR Strength;
              SHINE_CHAR_STATVAR Constitute;
              SHINE_CHAR_STATVAR Dexterity;
              SHINE_CHAR_STATVAR Intelligence;
              SHINE_CHAR_STATVAR Wizdom;
              SHINE_CHAR_STATVAR MentalPower;
              SHINE_CHAR_STATVAR WClow;
              SHINE_CHAR_STATVAR WChigh;
              SHINE_CHAR_STATVAR AC;
              SHINE_CHAR_STATVAR TH;
              SHINE_CHAR_STATVAR TB;
              SHINE_CHAR_STATVAR MAlow;
              SHINE_CHAR_STATVAR MAhigh;
              SHINE_CHAR_STATVAR MR;
              SHINE_CHAR_STATVAR MH;
              SHINE_CHAR_STATVAR MB;
              unsigned int MaxHp;
              unsigned int MaxSp;
              unsigned int MaxLp;
              unsigned int MaxAp;
              unsigned int MaxHPStone;
              unsigned int MaxSPStone;
              CHAR_PARAMETER_DATA::<unnamed-type-PwrStone> PwrStone;
              CHAR_PARAMETER_DATA::<unnamed-type-PwrStone> GrdStone;
              SHINE_CHAR_STATVAR PainRes;
              SHINE_CHAR_STATVAR RestraintRes;
              SHINE_CHAR_STATVAR CurseRes;
              SHINE_CHAR_STATVAR ShockRes;
            };
            */

            if (!IsLevelUp)
            {
                Packet.Write<ushort>(Character.MapObjectId);
            }

            //EXP
            Packet.Write<ulong>(Character.Info.EXP);
            Packet.Write<ulong>(Character.Info.ExpForNextLevel);

            var classParam = Character.Info.LevelParameter;

            Packet.Write<int>(classParam.Stats.Str); // base str
            Packet.Write<int>(Character.Stats.FullStats.Str);

            Packet.Write<int>(classParam.Stats.End); // base end
            Packet.Write<int>(Character.Stats.FullStats.End); // full end

            Packet.Write<int>(classParam.Stats.Dex); // base dex
            Packet.Write<int>(Character.Stats.FullStats.Dex); // full dex

            Packet.Write<int>(classParam.Stats.Int); // base int
            Packet.Write<int>(Character.Stats.FullStats.Int); // full int

            Packet.Write<int>(370); // wizdom. // 370
            Packet.Write<int>(3186); // wizdom? // 3186

            Packet.Write<int>(classParam.Stats.Spr); // base spr
            Packet.Write<int>(Character.Stats.FullStats.Spr); // full spr

            Packet.Write<int>(Character.Stats.NormalStats.WeaponDamage.Min); // base dmg min
            Packet.Write<int>(Character.Stats.NormalStats.WeaponDamage.Max); // base dmg max
            Packet.Write<int>(Character.Stats.FullStats.WeaponDamage.Min); // base magic dmg min
            Packet.Write<int>(Character.Stats.FullStats.WeaponDamage.Max); // base magic dmg max

            Packet.Write<int>(Character.Stats.NormalStats.WeaponDefense); // base def
            Packet.Write<int>(Character.Stats.FullStats.WeaponDefense); // increased def (e.g. buffs)

            Packet.Write<int>(Character.Stats.NormalStats.Aim); // base aim
            Packet.Write<int>(Character.Stats.FullStats.Aim); // increased aim

            Packet.Write<int>(Character.Stats.NormalStats.Evasion); // base Evasion
            Packet.Write<int>(Character.Stats.FullStats.Evasion); // increased Evasion

            Packet.Write<int>(Character.Stats.NormalStats.MagicDamage.Min); // increased dmg min
            Packet.Write<int>(Character.Stats.NormalStats.MagicDamage.Max); // increased dmg max
            Packet.Write<int>(Character.Stats.FullStats.MagicDamage.Min); // increased magic dmg min
            Packet.Write<int>(Character.Stats.FullStats.MagicDamage.Max); // increased magic dmg max

            Packet.Write<int>(Character.Stats.NormalStats.MagicDefense); // normal magic def
            Packet.Write<int>(Character.Stats.FullStats.MagicDefense); // increased  magic def

            Packet.Write<int>(1);
            Packet.Write<int>(20);
            Packet.Write<int>(2);
            Packet.Write<int>(40);

            // HP/SP/LightPower/AP
            Packet.Write<uint>(Character.Stats.FullStats.MaxHP);
            Packet.Write<uint>(Character.Stats.FullStats.MaxSP); 
            Packet.Write<uint>(Character.LivingStats.LP);
            Packet.Write<uint>(1);   //MaxAP

            // Max HP/SP Stones
            Packet.Write<uint>(Character.Info.MaxHPStones);
            Packet.Write<uint>(Character.Info.MaxSPStones); 

            Packet.Fill(64, 0); // buff bits ?

            if (!IsLevelUp)
            {
                Packet.Write<uint>(Character.Position.X);
                Packet.Write<uint>(Character.Position.Y);
            }
        }
    }
}