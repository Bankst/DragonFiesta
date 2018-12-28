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

		public static int GetTimeStamp(DateTime time)
		{
			return Convert.ToInt32((time - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds);
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
		}
	}
}
