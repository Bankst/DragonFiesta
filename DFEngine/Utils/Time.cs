using System;

namespace DFEngine.Utils
{
	/// <summary>
	/// Class that contains different time functions.
	/// </summary>
	public class Time
	{
		/// <summary>
		/// Represents the current system time in milliseconds.
		/// </summary>
		public static long Milliseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		/// <summary>
		/// Represents the current system time in seconds.
		/// </summary>
		public static long Seconds => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
	}
}
