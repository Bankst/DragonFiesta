using System.Data;

namespace DragonFiesta.Utils.Shine
{
	public class ShineTable : DataTable
	{
		private int unkColumns = 0;

		public ShineTable(string name)
		{
			this.TableName = name;
		}

		public void AddColumn(string name, string type)
		{
			this.Columns.Add(new ShineColumn(name, type, ref unkColumns));
		}

		public void AddRow(object[] data)
		{
			this.Rows.Add(data);
		}
	}
}
