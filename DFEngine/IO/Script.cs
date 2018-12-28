using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ported.VisualBasic.FileIO;

namespace DFEngine.IO
{
	/// <summary>
	/// Class that represents the contents of a Script file.
	/// </summary>
	public class Script : IDisposable
	{
		/// <summary>
		/// The script's rows, sorted by group.
		/// </summary>
		private readonly Dictionary<string, List<ScriptRow>> _rows;

		/// <summary>
		/// Creates a new instance of the <see cref="Script"/> class.
		/// </summary>
		/// <param name="path">The path of the file handle.</param>
		public Script(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException("Could not find the file specified.", path);
			}

			_rows = new Dictionary<string, List<ScriptRow>>();

			PopulateRows(path);
		}

		/// <summary>
		/// Safely obtains the rows associated with the specified group.
		/// </summary>
		/// <param name="group">The group to obtain.</param>
		public List<ScriptRow> this[string group] => _rows.GetSafe(group);

		/// <summary>
		/// Disposes the file handle.
		/// </summary>
		public void Dispose()
		{
			// empty...
		}

		/// <summary>
		/// Populate's the script's rows with the contents from the file.
		/// </summary>
		private void PopulateRows(string path)
		{
			using (var reader = new TextFieldParser(path))
			{
				reader.TrimWhiteSpace = true;
				reader.CommentTokens = new[] { ";" };
				reader.SetDelimiters(",", "\t", " ");
				reader.HasFieldsEnclosedInQuotes = true;

				while (!reader.EndOfData)
				{
					var row = reader.ReadFields();

					if (row == null || row.Length <= 0)
					{
						continue;
					}

					var group = row[0];

					if (group == "" || group.ToLower() == "#define" || group.ToLower() == "#enddefine")
					{
						continue;
					}

					if (group.ToLower() == "#include")
					{
						PopulateRows(row[1]);
						continue;
					}

					row = row.Where(s => s != group && s != "").ToArray();

					if (!_rows.ContainsKey(group))
					{
						_rows.Add(group, new List<ScriptRow>());
					}

					_rows[group].Add(new ScriptRow(row));
				}

				reader.Close();
			}
		}
	}
}
