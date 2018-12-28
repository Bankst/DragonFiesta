namespace DFEngine.Content.Other
{
	/// <summary>
	/// A class that represents a quickbar shortcut.
	/// </summary>
	public class Shortcut
	{
		/// <summary>
		/// The shortcut's slot.
		/// </summary>
		public byte Slot { get; set; }

		/// <summary>
		/// The shortcut's code.
		/// </summary>
		public ushort Code { get; set; }

		/// <summary>
		/// The shortcut's value.
		/// </summary>
		public ushort Value { get; set; }
	}
}
