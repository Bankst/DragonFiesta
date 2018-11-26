using System;

namespace DFEngine
{
	/// <summary>
	/// Class that extends the <see cref="System.DateTime"/> class,
	/// allowing us to declare additional methods to help us.
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Converts the DateTime instance to a 32-bit integer.
		/// </summary>
		public static int ToInt32(this DateTime value, bool neverIfNull = false)
		{
			var ret = 0;

			ret |= value.Minute << 0x19;
			ret |= (value.Hour & 0x3F) << 19;
			ret |= (value.Day & 0x3F) << 13;
			ret |= (value.Month & 0x1F) << 8;
			ret |= (byte)(value.Year - 2000);

			return neverIfNull && ret == 0 ? 1992027391 : ret;
		}
	}
}
