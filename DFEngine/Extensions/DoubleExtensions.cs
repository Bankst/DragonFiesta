using System;

namespace DFEngine
{
	public static class DoubleExtensions
	{
		public static byte ToDirectionByte(this double d)
		{
			if (d <= 0)
			{
				d += 360;
			}

			return Convert.ToByte(d / 2);
		}

		public static bool IsBetween(this double number, double min, double max)
		{
			return number >= min && number <= max;
		}
	}
}
