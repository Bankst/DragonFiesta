namespace DFEngine.IO.Definitions.SHN
{
	/// <summary>
	/// Class that defines the file SingleData.shn.
	/// </summary>
	[Definition]
	public class SingleData
	{
		/// <summary>
		/// The data's index.
		/// </summary>
		[Identity]
		public string SingleDataIDX { get; private set; }
		/// <summary>
		/// The data's value.
		/// </summary>
		public ushort SingleDataValue { get; private set; }
	}
}
