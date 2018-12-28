namespace DFEngine.IO.Definitions.SHN
{
	/// <summary>
	/// Class that defines the file FaceInfo.shn.
	/// </summary>
	[Definition]
	public class FaceInfo
	{
		/// <summary>
		/// The face's ID.
		/// </summary>
		[Identity]
		public byte ID { get; private set; }
		/// <summary>
		/// The face's grade.
		/// </summary>
		public byte Grade { get; private set; }
	}
}
