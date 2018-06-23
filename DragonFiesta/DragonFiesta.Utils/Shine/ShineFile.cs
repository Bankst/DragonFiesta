using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.Shine
{
	public class ShineFile
	{
		const string Comment = ";";
		const string StructInteger = "<integer>";
		const string StructString = "<string>";

		const string Replace = "#exchange";
		const string Ignore = "#ignore";
		const string StartDefine = "#define";
		const string EndDefine = "#enddefine";
		const string Table = "#table";
		const string ColumnName = "#columnname";
		const string ColumnType = "#columntype";
		const string Record = "#record";
		const string RecordLine = "#recordin"; // Contains tablename as first row.

		public uint ColumnCount { get; private set; }
		public uint RowCount { get; private set; }
		public string FileName { get; }

		public ShineFile(string filePath)
		{
			FileName = filePath;

			Read();
		}

		public Dictionary<string, ShineTable> TableDict { get; private set; }

		private void Read()
		{
			if (!File.Exists(FileName))
			{
				throw new FileNotFoundException("The file could not be found", FileName);
			}

			Dictionary<string, ShineTable> ret = new Dictionary<string, ShineTable>();

			using (var FileHandle = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var sr = new StreamReader(FileHandle, Encoding.Default))
				{
					bool definingStruct = false, definingTable = false;
					int lineNR = 0;
					ShineTable curTable = null;
					List<string> ColumnTypes = null;
					string comment = "";
					char? ignore = null;
					KeyValuePair<string, string>? replaceThis = null;
					while (!sr.EndOfStream)
					{
						lineNR++;
						string line = sr.ReadLine().TrimStart();
						if (line.Contains(Comment))
						{
							// Remove everything after it.
							int index = line.IndexOf(Comment);
							comment = line.Substring(index + 1);
							//Console.WriteLine("Comment @ {0}: {1}", lineNR, comment);

							line = line.Remove(index);
						}

						if (ignore.HasValue)
						{
							line = line.Replace(ignore.Value.ToString(), "");
						}

						if (line == string.Empty)
						{
							continue;
						}
						string lineLower = line.ToLower();
						string[] lineSplit = line.Split('\t');

						if (lineLower.StartsWith(Replace))
						{
							// ...
							replaceThis = new KeyValuePair<string, string>(ConvertShitToString(lineSplit[1]), ConvertShitToString(lineSplit[2])); // take risks :D
																																				  //continue;
						}
						if (lineLower.StartsWith(Ignore))
						{
							ignore = ConvertShitToString(lineSplit[1])[0];
							//continue;
						}

						if (lineLower.StartsWith(StartDefine))
						{
							if (definingStruct || definingTable)
							{
								throw new Exception("Already defining.");
							}
							// Get the name..
							string name = line.Substring(StartDefine.Length + 1);
							curTable = new ShineTable(name);

							definingStruct = true;
							continue;
						}
						else if (lineLower.StartsWith(Table))
						{
							if (definingStruct)
							{
								throw new Exception("Already defining.");
							}
							// Get the name..
							string name = lineSplit[1].Trim(); // I hope this works D;
							curTable = new ShineTable(name);
							ColumnTypes = new List<string>();
							ret.Add(name, curTable);
							definingTable = true;
							continue;
						}

						if (lineLower.StartsWith(EndDefine))
						{
							if (!definingStruct)
							{
								throw new Exception("Not started defining.");
							}
							definingStruct = false;
							ret.Add(curTable.TableName, curTable);
							continue;
						}

						line = line.Trim();
						lineLower = lineLower.Trim();

						if (definingStruct)
						{
							string columnName = comment.Trim();
							if (columnName == string.Empty) continue;
							curTable.AddColumn(columnName, lineLower);
							Console.WriteLine("Added column {0} to table {1}", columnName, curTable.TableName);
						}
						else if (definingTable)
						{
							// Lets search for columns..
							if (lineLower.StartsWith(ColumnType))
							{
								ColumnCount++;
								for (int i = 1; i < lineSplit.Length; i++)
								{
									string l = lineSplit[i].Trim();
									if (l == string.Empty) continue;
									ColumnTypes.Add(l);
								}
							}
							else if (lineLower.StartsWith(ColumnName))
							{
								int j = 0;
								for (int i = 1; i < lineSplit.Length; i++)
								{
									string l = lineSplit[i].Trim();
									if (l == string.Empty) continue;
									var coltype = ColumnTypes[j++];
									//curTable.AddColumn(l + "(" + coltype + ")", coltype);
									curTable.AddColumn(l, coltype);
								}
							}
							else if (lineLower.StartsWith(RecordLine))
							{
								// Next column is tablename
								string tablename = lineSplit[1].Trim();
								if (ret.ContainsKey(tablename))
								{
									curTable = ret[tablename];
									// Lets start.
									object[] data = new object[curTable.Columns.Count];
									int j = 0;
									for (int i = 2; i < lineSplit.Length; i++)
									{
										string l = lineSplit[i].Trim();
										if (l == string.Empty) continue;
										data[j++] = Check(replaceThis, l.TrimEnd(','));
									}
									curTable.AddRow(data);
								}
							}
							else if (lineLower.StartsWith(Record))
							{
								RowCount++;
								// Right under the table
								object[] data = new object[curTable.Columns.Count];
								int j = 0;
								for (int i = 1; i < lineSplit.Length; i++)
								{
									string l = lineSplit[i].Trim();
									if (l == string.Empty) continue;
									data[j++] = Check(replaceThis, l.TrimEnd(','));
								}
								curTable.AddRow(data);
							}
						}
						else
						{
							if (ret.ContainsKey(lineSplit[0].Trim()))
							{
								// Should be a struct I guess D:
								var table = ret[lineSplit[0].Trim()];
								int columnsInStruct = table.Columns.Count;
								int readColumns = 0;
								object[] data = new object[columnsInStruct];
								for (int i = 1; ; i++)
								{
									if (readColumns == columnsInStruct)
									{
										break;
									}
									else if (lineSplit.Length < i)
									{
										throw new Exception(string.Format("Could not read all columns of line {0}", lineNR));
									}
									// Cannot count on the tabs ...
									string columnText = lineSplit[i].Trim();
									if (columnText == string.Empty) continue;
									// Well, lets see if we can put it into the list
									columnText = columnText.TrimEnd(',').Trim('"');

									data[readColumns++] = columnText;
								}
								table.AddRow(data);
							}
						}

					}
				}
			}
			TableDict = ret;
		}

		private string ConvertShitToString(string input)
		{
			// HACKZ IN HERE
			if (input.StartsWith("\\x"))
			{
				return ((char)Convert.ToByte(input.Substring(2), 16)).ToString();
			}
			else if (input.StartsWith("\\o"))
			{
				return ((char)Convert.ToByte(input.Substring(2), 8)).ToString();
			}

			return input.Length > 0 ? input[0].ToString() : "";
		}

		private string Check(KeyValuePair<string, string>? replacing, string input)
		{
			return replacing.HasValue ? input.Replace(replacing.Value.Key, replacing.Value.Value) : input;
		}
	}
}
