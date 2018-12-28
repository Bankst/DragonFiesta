namespace DFEngine.IO.Definitions.SHN
{
	/// <summary>
	/// Class that defines the file HairInfo.shn.
	/// </summary>
	[Definition]
	public class HairInfo
	{
		/// <summary>
		/// The hair's ID.
		/// </summary>
		[Identity]
		public byte ID { get; private set; }
		/// <summary>
		/// The hair's grade.
		/// </summary>
		public byte Grade { get; private set; }
	}
}
