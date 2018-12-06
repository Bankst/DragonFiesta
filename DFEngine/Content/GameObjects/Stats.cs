namespace DFEngine.Content.GameObjects
{
	public class Stats
	{
		public int CurrentHP { get; set; }
		public int CurrentSP { get; set; }
		public int CurrentLP { get; set; }

		public int CurrentMaxHP { get; set; }
		public int CurrentMaxSP { get; set; }
		public int CurrentMaxLP { get; set; }

		public int CurrentHPStones { get; set; }
		public int CurrentSPStones { get; set; }

		public int CurrentMaxHPStones { get; set; }
		public int CurrentMaxSPStones { get; set; }

		public byte FreeSTR { get; set; }
		public byte FreeEND { get; set; }
		public byte FreeDEX { get; set; }
		public byte FreeINT { get; set; }
		public byte FreeSPR { get; set; }

		public void Update()
		{

		}
	}
}
