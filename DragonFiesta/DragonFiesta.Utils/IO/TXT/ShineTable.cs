using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.IO.TXT
{
	public class ShineTable
	{
		public String LoadPath;

		public List<Table> Tables;

		public ShineTable(String LP) { LoadPath = LP; }

		public Boolean IsShineTable()
		{
			foreach (String Line in File.ReadAllLines(LoadPath))
			{
				if (Line.ToLower().StartsWith("#table")) { return true; }
			}

			return false;
		}

		public void Read()
		{
			Tables = new List<Table>();

			StreamReader Reader = new StreamReader(LoadPath);

			Table NewTable = new Table();

			String CurrentLine;

			while ((CurrentLine = Reader.ReadLine()) != null)
			{
				if (CurrentLine.ToLower().StartsWith("#table"))
				{
					NewTable = new Table();
					NewTable.Source = new DataTable();
					NewTable.Source.TableName = CurrentLine.Split(new String[] { "\t" }, StringSplitOptions.None)[1];

					Tables.Add(NewTable);
				}
				else if (CurrentLine.ToLower().StartsWith("#columntype")) { NewTable.ColumnTypeRow = CurrentLine; }
				else if (CurrentLine.ToLower().StartsWith("#columnname"))
				{
					Int32 ID = 0;

					String[] ColumnSplit = CurrentLine.Split(new String[] { "\t" }, StringSplitOptions.None);

					if (CurrentLine.Contains(";")) { CurrentLine = CurrentLine.Split(';')[0]; }

					for (Int32 Counter = 1; Counter < ColumnSplit.Length; Counter++)
					{
						if ((!ColumnSplit[Counter].Contains(";") || ColumnSplit[Counter] != ";") && ColumnSplit[Counter] != String.Empty)
						{
							DataColumn NewColumn = new DataColumn();
							NewColumn.ColumnName = ColumnSplit[Counter];

							NewTable.Source.Columns.Add(NewColumn);

							ID++;
						}
					}
				}
				else if (CurrentLine.ToLower().StartsWith("#record") || CurrentLine.ToLower().StartsWith("#recordin"))
				{
					List<String> ItemArray = new List<String>();

					if (CurrentLine.Contains(";")) { CurrentLine = CurrentLine.Split(':')[0]; }

					String[] RowSplit = CurrentLine.Split(new String[] { "\t" }, StringSplitOptions.None);

					for (Int32 Counter = 1; Counter < RowSplit.Length; Counter++)
					{
						if ((!RowSplit[Counter].Contains(";") || RowSplit[Counter] != ";") && RowSplit[Counter] != String.Empty) { ItemArray.Add(RowSplit[Counter]); }
					}

					NewTable.Source.Rows.Add(ItemArray.ToArray());
				}
				else if (CurrentLine.ToLower().StartsWith("#end")) { break; }
			}

			Reader.Dispose();
		}

		public void Write(String WritePath)
		{
			if (File.Exists(WritePath)) { File.Move(WritePath, String.Format("C:\\SHNBackups\\{0}{1}.txt", Path.GetFileNameWithoutExtension(WritePath), DateTime.Now.ToString("dd-MM-yyyy.hh-mm-ss-fff"))); }

			StreamWriter Writer = new StreamWriter(File.Open(WritePath, FileMode.Create, FileAccess.Write, FileShare.None));
			Headers.WriteHeaders(Writer, Tables[0].Source.TableName);

			foreach (Table CurrentTable in Tables)
			{
				Writer.WriteLine();
				Writer.WriteLine("#Table\t{0}", CurrentTable.Source.TableName);
				Writer.WriteLine(CurrentTable.ColumnTypeRow);
				Writer.Write("#ColumnName\t");

				for (Int32 Counter = 0; Counter < CurrentTable.Source.Columns.Count; Counter++)
				{
					if (Counter == (CurrentTable.Source.Columns.Count - 1)) { Writer.Write(CurrentTable.Source.Columns[Counter]); }
					else { Writer.Write("{0}\t", CurrentTable.Source.Columns[Counter].ColumnName); }
				}

				Writer.WriteLine();

				foreach (DataRow CurrentRow in CurrentTable.Source.Rows)
				{
					Writer.Write("#Record\t");

					for (Int32 Counter = 0; Counter < CurrentTable.Source.Columns.Count; Counter++)
					{
						if (Counter == (CurrentTable.Source.Columns.Count - 1)) { Writer.Write(CurrentRow[Counter]); }
						else { Writer.Write("{0}\t", CurrentRow[Counter]); }
					}

					Writer.WriteLine();
				}
			}

			Writer.WriteLine();
			Writer.WriteLine("#End");
			Writer.Flush();

			Writer.Dispose();

			LoadPath = WritePath;
		}
	}
}
