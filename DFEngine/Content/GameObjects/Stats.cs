namespace DFEngine.Content.GameObjects
{
	public class Stats
	{
		public short CurrentHP { get; set; }
		public short CurrentSP { get; set; }
		public short CurrentLP { get; set; }
		public short CurrentHPStones { get; set; }
		public short CurrentSPStones { get; set; }

		public byte FreeSTR { get; set; }
		public byte FreeEND { get; set; }
		public byte FreeDEX { get; set; }
		public byte FreeINT { get; set; }
		public byte FreeSPR { get; set; }
	}
}
