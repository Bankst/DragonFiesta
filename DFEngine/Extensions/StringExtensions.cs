using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DFEngine
{
	public static class StringExtensions
	{
		public static bool ParseBool(string input, out bool result) => result = input.Equals("1");

		public static string ToEscapedString(this string input, string escapeSign = "\\")
		{
			return (input + escapeSign).Replace(escapeSign + escapeSign, escapeSign);
		}

		public static string ToAbsolutePath(this string input, string baseDirectory = null)
		{
			var path = input;

			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(Path.Combine((baseDirectory ?? AppDomain.CurrentDomain.BaseDirectory), input));
			}

			return path;
		}

		public static string ToString(this string[] input)
		{
			//
			// Concatenate all the elements into a StringBuilder.
			//
			var builder = new StringBuilder();
			foreach (var value in input)
			{
				builder.Append(value);
				builder.Append(' ');
			}
			return builder.ToString();
		}

		public static string FormatEx(this string s, params object[] parameters)
		{
			var r = new Regex(Regex.Escape("{*}"));

			return parameters.Aggregate(s, (current, param) => r.Replace(current, param.ToString(), 1));
		}

		public static string ToFiestaString(this string input, params Tuple<string, string>[] replacers)
		{
			var outSring = input;

			foreach (var rep in replacers)
			{
				var inx = outSring.IndexOf(rep.Item1, StringComparison.Ordinal);

				if (inx < 0) continue;
				outSring = outSring.Remove(inx, rep.Item1.Length);
				outSring = outSring.Insert(inx, rep.Item2);
			}

			return outSring;
		}

		public static string ToMD5(this string input)
		{
			var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
			var stringBuilder = new StringBuilder();
			foreach (var num in hash)
				stringBuilder.Append(num.ToString("x2"));
			return stringBuilder.ToString();
		}

		public static byte ToByte(this string input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));
			if (input.Length == 0 || 2 < input.Length)
				throw new ArgumentOutOfRangeException(nameof(input), "The hex string must be 1 or 2 characters in length.");
			return byte.Parse(input, NumberStyles.HexNumber);
		}

		public static byte[] ToBytes(this string input)
		{
			var stringBuilder = new StringBuilder();
			foreach (var ch in input.Where(CharExtensions.IsHexDigit).Select(char.ToUpper))
				stringBuilder.Append(ch);
			var str = stringBuilder.ToString();
			if (str.Length % 2 == 1)
				str += "0";
			var numArray = new byte[str.Length / 2];
			var index1 = 0;
			var index2 = 0;
			while (index1 < numArray.Length)
			{
				var input1 = ((int)str[index2]).ToString() + str[index2 + 1];
				numArray[index1] = input1.ToByte();
				++index1;
				index2 += 2;
			}
			return numArray;
		}
	}
}