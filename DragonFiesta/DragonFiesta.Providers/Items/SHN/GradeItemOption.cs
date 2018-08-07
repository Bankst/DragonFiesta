using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
	public class GradeItemOption
	{
		public string ItemIndex { get; }
		public uint STR { get; }
		public uint CON { get; }
		public uint DEX { get; }
		public uint INT { get; }
		public uint MEN { get; }
		public uint ResistPoison { get; }
		public uint ResistDisease { get; } // 'ResistDeaseas' in SHN
		public uint ResistCurse { get; }
		public uint ResistMoveSpdDown { get; }
		public uint ToHitRate { get; }
		public uint ToBlockRate { get; }
		public uint MaxHP { get; }
		public uint MaxSP { get; }
		public uint WCPlus { get; }
		public uint MAPlus { get; }

		public GradeItemOption(SHNResult pResult, int i)
		{
			ItemIndex = pResult.Read<string>(i, "ItemIndex");
			STR = pResult.Read<uint>(i, "STR");
			CON = pResult.Read<uint>(i, "CON");
			DEX = pResult.Read<uint>(i, "DEX");
			INT = pResult.Read<uint>(i, "INT");
			MEN = pResult.Read<uint>(i, "MEN");
			ResistPoison = pResult.Read<uint>(i, "ResistPoison");
			ResistDisease = pResult.Read<uint>(i, "ResistDeaseas");
			ResistCurse = pResult.Read<uint>(i, "ResistCurse");
			ResistMoveSpdDown = pResult.Read<uint>(i, "ResistMoveSpdDown");
			ToHitRate = pResult.Read<uint>(i, "ToHitRate");
			ToBlockRate = pResult.Read<uint>(i, "ToBlockRate");
			MaxHP = pResult.Read<uint>(i, "MaxHP");
			MaxSP = pResult.Read<uint>(i, "MaxSP");
			WCPlus = pResult.Read<uint>(i, "WCPlus");
			MAPlus = pResult.Read<uint>(i, "MAPlus");
		}
	}
}