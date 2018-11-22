using System;

namespace DFEngine
{
	public static class UnixTimestamp
	{
		public static double GetCurrent()
		{
			var ts = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
			return ts.TotalSeconds;
		}

		public static DateTime GetDateTimeFromUnixTimestamp(double timestamp)
		{
			var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			dt = dt.AddSeconds(timestamp);
			return dt;
		}
	}
}
