using DragonFiesta.Zone.Game.Stats;
using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Game.Character
{
    public class CharacterFreeStatsInfo
    {
        public byte FreeStat_Points { get; set; }

        public byte StatPoints_STR { get; set; }

        public byte StatPoints_END { get; set; }

        public byte StatPoints_DEX { get; set; }

        public byte StatPoints_INT { get; set; }

        public byte StatPoints_SPR { get; set; }


        public FreeStats Stats { get; set; }

        private ZoneCharacter Owner { get; set; }

        public CharacterFreeStatsInfo(ZoneCharacter Owner)
        {
            Stats = new FreeStats();
            this.Owner = Owner;
        }

	    public bool FreeStatsInfo()
	    {
		    try
		    {
			    StatPoints_STR = Owner.FreeStat_Str;
			    StatPoints_END = Owner.FreeStat_End;
			    StatPoints_DEX = Owner.FreeStat_Dex;
			    StatPoints_INT = Owner.FreeStat_Int;
			    StatPoints_SPR = Owner.FreeStat_Spr;

			    FreeStat_Points = Owner.FreeStat_Points;

			    return true;
		    }
		    catch (Exception ex)
		    {
			    DatabaseLog.Write(ex, "Failed Load CharacterFreeStats");
			    return false;
		    }
		}

        public bool FreeStatsInfo(SQLResult pRes, int i)
        {
            try
            {
                StatPoints_STR = pRes.Read<byte>(i, "FreeStat_Str");
                StatPoints_END = pRes.Read<byte>(i, "FreeStat_End");
                StatPoints_DEX = pRes.Read<byte>(i, "FreeStat_Dex");
                StatPoints_INT = pRes.Read<byte>(i, "FreeStat_Int");
                StatPoints_SPR = pRes.Read<byte>(i, "FreeStat_Spr");


                FreeStat_Points = pRes.Read<byte>(i, "FreeStat_Points");

                return true;
            }
            catch(Exception ex)
            {
                DatabaseLog.Write(ex, "Failed Load CharacterFreeStats");
                return false;
            }
        }
    }
}
