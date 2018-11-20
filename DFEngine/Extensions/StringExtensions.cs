using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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

		foreach (var param in parameters)
		{
			s = r.Replace(s, param.ToString(), 1);
		}

		return s;
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
}