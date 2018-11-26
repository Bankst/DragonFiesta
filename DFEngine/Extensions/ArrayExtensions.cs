using System;

namespace DFEngine
{
	/// <summary>
	/// Class that extends the use of the Array class.
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		/// Compares two byte arrays of values.
		/// </summary>
		public static bool Compare(this byte[] first, byte[] second)
		{
			var firstString = Convert.ToBase64String(first);
			var secondString = Convert.ToBase64String(second);

			return firstString == secondString;
		}
	}
}
