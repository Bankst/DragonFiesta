using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static bool ParseBool(string Input, out bool Result) => Result = (Input.Equals("1") ? true : false);

    public static string ToEscapedString(this string Input, string EscapeSign = "\\")
    {
        return (Input + EscapeSign).Replace(EscapeSign + EscapeSign, EscapeSign);
    }

    public static string ToAbsolutePath(this string Input, string BaseDirectory = null)
    {
        var path = Input;

        if (!Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(Path.Combine((BaseDirectory ?? AppDomain.CurrentDomain.BaseDirectory), Input));
        }

        return path;
    }

    public static string ToTrimedLine(this string Input)
    {
        var output = Input.Replace("\"", "").Replace(",", "").Trim();

        while (output.Contains("		"))
        {
            output = output.Replace("		", "	");
        }

        return output;
    }

    public static string ToString(this string[] Input)
    {
        //
        // Concatenate all the elements into a StringBuilder.
        //
        StringBuilder builder = new StringBuilder();
        foreach (string value in Input)
        {
            builder.Append(value);
            builder.Append(' ');
        }
        return builder.ToString();
    }

    public static string FormatEx(this string s, params object[] parameters)
    {
        Regex r = new Regex(Regex.Escape("{*}"));

        for (int i = 0; i < parameters.Length; i++)
        {
            s = r.Replace(s, parameters[i].ToString(), 1);
        }

        return s;
    }

    public static string ToFiestaString(this string Input, params Tuple<string, string>[] Replacers)
    {
        var outSring = Input;

        for (int i = 0; i < Replacers.Length; i++)
        {
            var rep = Replacers[i];
            var inx = outSring.IndexOf(rep.Item1);

            if (inx >= 0)
            {
                outSring = outSring.Remove(inx, rep.Item1.Length);
                outSring = outSring.Insert(inx, rep.Item2);
            }
        }

        return outSring;
    }
}