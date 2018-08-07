using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.KingdomQuest
{
    public class KQTeam
    {
        public short ID { get; }

        public byte MaxMemberGap { get; }

        public byte IsTeamPVP { get; }

        public short KQTeamDivideType { get; }

        public uint RegenXRed { get; }

        public uint RegenYRed { get; }

        public uint RegenXBlue { get; }

        public uint RegenYBlue { get; }

        public KQTeam(SHNResult pResult, int i)
        {
            ID = pResult.Read<short>(i, "ID");
            MaxMemberGap = pResult.Read<byte>(i, "MaxMemberGap");
            IsTeamPVP = pResult.Read<byte>(i, "IsTeamPVP");
            KQTeamDivideType = pResult.Read<short>(i, "KQTeamDivideType");
            RegenXRed = pResult.Read<uint>(i, "RegenXRed");
            RegenYRed = pResult.Read<uint>(i, "RegenYRed");
            RegenXBlue = pResult.Read<uint>(i, "RegenXBlue");
            RegenYBlue = pResult.Read<uint>(i, "RegenYBlue");
        }
    }
}
