using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Utils.IO.TXT
{
	public class TXTManager
	{
		public const string DataPath = "Shine";
		public static List<ShineTable> LoadMany(TXTType type)
		{
			var tablesPath = $"{TXTPaths.GetPath(type, DataPath)}";
			var dirInfo = new DirectoryInfo(tablesPath);
			var filesList = dirInfo.GetFiles("*.txt");
			foreach (var file in filesList)
			{
				DatabaseLog.Write(DatabaseLogLevel.Debug, $"Loading ShineTable. Type: {type.ToString()}  Name: {file.Name}");
				continue;
			}

			return null; // for now
		}

		public static List<TXTTable> LoadSingle(TXTType type, string fileName)
		{
			var txtPath = $"{TXTPaths.GetPath(type, DataPath)}/{fileName}.txt";
			var txtFile = new ShineTable(txtPath);
			txtFile.Read();

			var tables = new List<TXTTable>();

			foreach (var table in txtFile.Tables)
			{
				using (var txtTableDataReader = table.Source.CreateDataReader())
				{
					using (var txtData = new TXTTable())
					{
						txtData.Load(txtTableDataReader);
						txtData.Count = table.Source.Rows.Count;
						tables.Add(txtData);
					}
				}
				
			}
			return tables;
		}

	}
}
