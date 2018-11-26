namespace DFEngine.IO.Definitions.SHN
{
	/// <summary>
	/// Class that defines the file HairColorInfo.shn.
	/// </summary>
	[Definition]
	public class HairColorInfo
	{
		/// <summary>
		/// The hair color's ID.
		/// </summary>
		[Identity]
		public byte ID { get; private set; }
		/// <summary>
		/// The hair color's grade.
		/// </summary>
		public byte Grade { get; private set; }
	}
}
