namespace DFEngine.Content.Items
{
	public class WeaponLicense
	{
		public byte Grade { get; set; }

		public ushort MobID { get; set; }

		public int KillCount { get; set; }

		public WeaponLicense(byte grade, ushort mobID, int killCount)
		{
			Grade = grade;
			MobID = mobID;
			KillCount = killCount;
		}
	}
}
