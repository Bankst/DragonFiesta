using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DFEngine.Logging;

namespace DFEngine.IO
{
	/// <summary>
	/// Class to load and access Shine tables.
	/// </summary>
	public class ShineTable : IDisposable
	{
		/// <summary>
		/// Represents a comment.
		/// </summary>
		private const string COMMENT = ";";
		/// <summary>
		/// Represents a replacement in the file.
		/// </summary>
		private const string REPLACE = "#exchange";
		/// <summary>
		/// Represents an ignored part of the file.
		/// </summary>
		private const string IGNORE = "#ignore";
		/// <summary>
		/// Represents the file's start of definitions.
		/// </summary>
		private const string DEFINE = "#define";
		/// <summary>
		/// Represents the file's end of definitions.
		/// </summary>
		private const string ENDDEFINE = "#enddefine";
		/// <summary>
		/// Represents a table indicator.
		/// </summary>
		private const string TABLE = "#table";
		/// <summary>
		/// Represents the name of a column.
		/// </summary>
		private const string COLUMN_NAME = "#columnname";
		/// <summary>
		/// Represents the type of a column.
		/// </summary>
		private const string COLUMN_TYPE = "#columntype";
		/// <summary>
		/// Represents a record in the file.
		/// </summary>
		private const string RECORD = "#record";
		/// <summary>
		/// Represents a record in a table.
		/// </summary>
		private const string RECORD_IN = "#recordin";

		/// <summary>
		/// The name of the file we're reading from.
		/// </summary>
		private readonly string fileName;

		/// <summary>
		/// All of the tables in the file handle.
		/// </summary>
		private readonly Dictionary<string, DataTable> tables;

		/// <summary>
		/// Creates a new instance of the <see cref="ShineTable"/> class.
		/// </summary>
		/// <param name="path">The path of the file.</param>
		public ShineTable(string path)
		{
			fileName = path;
			tables = new Dictionary<string, DataTable>();

			PopulateTables();
		}

		/// <summary>
		/// Returns the table with the associated table name.
		/// </summary>
		/// <param name="tableName">The name of the table.</param>
		public DataTable this[string tableName] => tables.GetSafe(tableName);

		/// <summary>
		/// Disposes the file handle.
		/// </summary>
		public void Dispose()
		{
			foreach (var table in tables.Values)
			{
				table.Dispose();
			}

			tables.Clear();
		}

		/// <summary>
		/// Checks a value for replacements.
		/// </summary>
		private string CheckValue(Tuple<string, string> replacing, string input)
		{
			return replacing != null ? input.Replace(replacing.Item1, replacing.Item2) : input;
		}

		/// <summary>
		/// Converts the input string to a readable string.
		/// </summary>
		private string ConvertString(string input)
		{
			if (input.StartsWith("\\x"))
			{
				return ((char)Convert.ToByte(input.Substring(2), 16)).ToString();
			}

			if (input.StartsWith("\\o"))
			{
				return ((char)Convert.ToByte(input.Substring(2), 8)).ToString();
			}

			return input.Length > 0 ? input[0].ToString() : "";
		}

		/// <summary>
		/// Gets the data type for a column.
		/// </summary>
		/// <param name="type">The name of the column's data type.</param>
		private Type GetColumnDataType(string type)
		{
			var typeName = type.ToLower();
			if (typeName.StartsWith("string["))
			{
				return typeof(string);
			}

			switch (typeName)
			{
				case "byte":
					return typeof(byte);
				case "word":
					return typeof(short);
				case "<integer>":
				case "dwrd":
				case "dword":
					return typeof(int);
				case "qword":
					return typeof(long);
				case "index":
					return typeof(string);
				case "<string>":
				case "string":
					return typeof(string);
			}

			return typeof(string);
		}

		/// <summary>
		/// Populates the file's tables with data from the file.
		/// </summary>
		private void PopulateTables()
		{
			if (!File.Exists(fileName))
			{
				EngineLog.Write(EngineLogLevel.Warning, $"The ShineTable file '{fileName}' could not be found.");
				return;
			}

			using (var file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var reader = new StreamReader(file, Encoding.Default))
			{
				var definingStruct = false;
				var definingTable = false;
				var lineNumber = 0;
				DataTable currentTable = null;
				List<string> columnTypes = null;
				var rowComment = "";
				char? ignoreThis = null;
				Tuple<string, string> replaceThis = null;
				var unkColumnCount = 0;

				while (!reader.EndOfStream)
				{
					lineNumber++;
					string line;

					if ((line = reader.ReadLine()) == null)
					{
						break;
					}

					if (line.Contains(COMMENT))
					{
						var index = line.IndexOf(COMMENT, StringComparison.Ordinal);
						rowComment = line.Substring(index + 1);
						line = line.Remove(index);
					}

					if (ignoreThis.HasValue)
					{
						line = line.Replace(ignoreThis.Value.ToString(), "");
					}

					if (line == string.Empty)
					{
						continue;
					}

					var lineLower = line.ToLower();
					var items = line.Split('\t');

					if (lineLower.StartsWith(REPLACE))
					{
						replaceThis = new Tuple<string, string>(ConvertString(items[0]), ConvertString(items[2]));
					}

					if (lineLower.StartsWith(IGNORE))
					{
						ignoreThis = ConvertString(items[1])[0];
					}

					if (lineLower.StartsWith(DEFINE))
					{
						if (definingStruct || definingTable)
						{
							throw new Exception("Something is already being defined.");
						}

						var name = line.Substring(DEFINE.Length + 1);
						currentTable = new DataTable(name);

						definingStruct = true;
						continue;
					}

					if (lineLower.StartsWith(TABLE))
					{
						if (definingStruct)
						{
							throw new Exception("Something is already being defined.");
						}

						var name = items[1].Trim();
						currentTable = new DataTable(name);
						columnTypes = new List<string>();
						unkColumnCount = 0;

						tables.Add(name, currentTable);
						definingTable = true;
						continue;
					}

					if (lineLower.StartsWith(ENDDEFINE))
					{
						if (!definingStruct)
						{
							throw new Exception("A structure hasn't started being defined.");
						}

						tables.Add(currentTable.TableName, currentTable);
						definingStruct = false;
						continue;
					}

					lineLower = lineLower.Trim();

					if (definingStruct)
					{
						var columnName = rowComment.Trim();
						if (columnName == string.Empty)
						{
							continue;
						}

						var column = new DataColumn(columnName, GetColumnDataType(lineLower));

						if (columnName.Length < 2)
						{
							column.ColumnName = $"UnkCol{unkColumnCount++}";
						}

						currentTable.Columns.Add(column);
					}
					else if (definingTable)
					{
						if (lineLower.StartsWith(COLUMN_TYPE))
						{
							for (var i = 1; i < items.Length; i++)
							{
								var type = items[i].Trim();

								if (type == string.Empty)
								{
									continue;
								}

								columnTypes.Add(type);
							}
						}
						else if (lineLower.StartsWith(COLUMN_NAME))
						{
							for (int i = 1, j = 0; i < items.Length; i++)
							{
								var name = items[i].Trim();

								if (name == string.Empty)
								{
									continue;
								}

								var columnType = columnTypes[j++];
								var column = new DataColumn(name, GetColumnDataType(columnType));

								if (name.Length < 2)
								{
									column.ColumnName = $"UnkCol{unkColumnCount++}";
								}

								currentTable.Columns.Add(column);
							}
						}
						else if (lineLower.StartsWith(RECORD_IN))
						{
							var tableName = items[1].Trim();

							if (tables.ContainsKey(tableName))
							{
								currentTable = tables[tableName];
								var rowData = new object[currentTable.Columns.Count];

								for (int i = 2, j = 0; i < items.Length; i++)
								{
									var item = items[i].Trim();
									if (item == string.Empty)
									{
										continue;
									}

									rowData[j++] = CheckValue(replaceThis, item.TrimEnd(','));
								}

								currentTable.Rows.Add(rowData);
							}
						}
						else if (lineLower.StartsWith(RECORD))
						{
							var rowData = new object[currentTable.Columns.Count];

							for (int i = 1, j = 0; i < items.Length; i++)
							{
								if (j >= currentTable.Columns.Count)
								{
									break;
								}

								var item = items[i].Trim();
								if (item == string.Empty)
								{
									continue;
								}

								rowData[j++] = CheckValue(replaceThis, item.TrimEnd(','));
							}

							currentTable.Rows.Add(rowData);
						}
					}
					else
					{
						if (tables.ContainsKey(items[0].Trim()))
						{
							var existingTable = tables[items[0].Trim()];
							var columnCount = existingTable.Columns.Count;
							var readColumns = 0;
							var rowData = new object[columnCount];

							for (var i = 1; ; i++)
							{
								if (readColumns == columnCount)
								{
									break;
								}

								if (items.Length < i)
								{
									throw new Exception($"Could not read all columns of line {lineNumber}");
								}

								var columnText = items[i].Trim();
								if (columnText == string.Empty)
								{
									continue;
								}

								columnText = columnText.TrimEnd(',').Trim('"');
								rowData[readColumns++] = columnText;
							}

							existingTable.Rows.Add(rowData);
						}
					}
				}
			}
		}
	}
}
