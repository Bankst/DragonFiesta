﻿using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Zone.Data.Character;
using DragonFiesta.Zone.Game.Stats;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using System;

namespace DragonFiesta.Zone.Game.Character
{
    public class ZoneCharacterInfo : CharacterInfo
    {
        private ZoneCharacter Character { get; set; }

        private ushort _HPStones;

        private ushort _SPStones;



        public CharacterFreeStatsInfo FreeStats { get; private set; }

        public CharacterStatsManager Stats { get; private set; }



        public CharacterLevelParameter LevelParameter { get; set; }

        public ushort MaxHPStones { get; set; }
        public ushort MaxSPStones { get; set; }


        public ushort HPStones
        {
            get => _HPStones;
            set
            {
                _HPStones = Math.Min(MaxHPStones, value);

                if (Character.IsConnected
                    && Character.Session.Ingame)
                {
                    SH20Handler.SendUpdateHPStones(Character);
                }
            }
        }

        public ushort SPStones
        {
            get => _SPStones;
            set
            {
                _SPStones = Math.Min(MaxSPStones, value);

                if (Character.IsConnected
                    && Character.Session.Ingame)
                {
                    SH20Handler.SendUpdateSPStones(Character);
                }
            }
        }

        public override ulong Money
        {
            get => base.Money;

            set
            {
                base.Money = value;

                if (Character.IsConnected)
                {
                    SH04Handler.SendUpdateMoney(Character);

                    CharacterMethods.SendCharacterUpdateMoney(Character);
                }
            }
        }


        public uint Fame { get; set; }

        public new ushort FriendPoints
        {
            get => base.FriendPoints;
            set=>  base.FriendPoints = Math.Min(value, ushort.MaxValue);
        }
        public uint KillPoints { get; private set; }

        public byte SkillPoints { get; set; }

        public ZoneCharacterInfo(ZoneCharacter Character) : base()
        {
            this.Character = Character;
        }

        public override bool RefreshFromSQL(SQLResult pRes, int i)
        {
            if (!base.RefreshFromSQL(pRes, i))
                return false;



            CharacterLevelParameter Parameter;
            if (!CharacterDataProvider.GetLevelParameters(Class, Level, out Parameter))
                return false;

            LevelParameter = Parameter;


            ExpForNextLevel = CharacterDataProvider.GetEXPForNextLevel(Level);

            MaxHPStones = Parameter.MaxHPStones;
            MaxSPStones = Parameter.MaxSPStones;

            HPStones = pRes.Read<ushort>(i, "HPStones");
            SPStones = pRes.Read<ushort>(i, "SPStones");




            FreeStats = new CharacterFreeStatsInfo(Character);

            if (!FreeStats.FreeStatsInfo(pRes, i))
                return false;


            Stats = new CharacterStatsManager(Character);

            Stats.UpdateAll();

            KillPoints = pRes.Read<uint>(i, "KillPoints");

            EXP = pRes.Read<ulong>(i, "EXP");

            Fame = pRes.Read<uint>(i, "Fame");

            SkillPoints = pRes.Read<byte>(i, "SkillPoints");

            return true;
        }

        ~ZoneCharacterInfo()
        {
            Character = null;
            Stats.Dispose();
            Stats = null;
            FreeStats = null;
            LevelParameter = null;
        }
    }
}