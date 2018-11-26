using System;
using System.Security.Cryptography;

namespace DFEngine.Utils
{
	/// <summary>
	/// Class that contains commonly used math functions.
	/// </summary>
	public class Mathd
	{
		/// <summary>
		/// Degrees-to-radians conversion constant.
		/// </summary>
		public static double Deg2Rad = ((float)Math.PI * 2) / 360;

		/// <summary>
		/// Radians-to-degrees conversion constant.
		/// </summary>
		public static double Rad2Deg = 360 / ((float)Math.PI * 2);

		/// <summary>
		/// Represents the value of 2π
		/// </summary>
		public static double TwoPi = (float)Math.PI * 2;

		/// <summary>
		/// A class instance the produces random data.
		/// </summary>
		private static readonly RandomNumberGenerator RNG = RandomNumberGenerator.Create();

		///<summary>
		/// Returns a random number within the specified range.
		///</summary>
		///<param name="minValue">The inclusive lower bound of the random number returned.</param>
		///<param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		public static int Random(int minValue, int maxValue)
		{
			return (int)Math.Round(RandomDouble() * (maxValue - minValue)) + minValue;
		}

		///<summary>
		/// Returns a nonnegative random number.
		///</summary>
		public static int Random()
		{
			return Random(0, int.MaxValue);
		}

		///<summary>
		/// Returns a nonnegative random number less than the specified maximum
		///</summary>
		///<param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal 0</param>
		public static int Random(int maxValue)
		{
			return Random(0, maxValue);
		}

		/// <summary>
		/// Returns a random boolean based on the probability.
		/// </summary>
		/// <param name="probability">The probability of the boolean being true.</param>
		public static bool RandomBool(double probability = 50)
		{
			return Random(100) <= probability;
		}

		///<summary>
		/// Returns a random number between 0.0 and 1.0.
		///</summary>
		public static double RandomDouble()
		{
			var buffer = new byte[4];
			RNG.GetBytes(buffer);

			return (double)BitConverter.ToUInt32(buffer, 0) / uint.MaxValue;
		}
	}
}
