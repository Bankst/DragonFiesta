using System;
using System.Data;
using System.Globalization;

namespace DragonFiesta.Utils.IO.TXT
{
	public class TXTTable : DataTable
	{
		public int Count { get; set; }

		public T Read<T>(int row, string columnName, int number = 0)
			where T : IConvertible
		{
			return ToOrDefault<T>(Rows[row], columnName + (number != 0 ? (1 + number).ToString() : ""));
		}

		public bool HasValues
		{
			get
			{
				if (!Count.Equals(0))
					return true;

				return false;
			}
		}

		public byte[] GetBytes(int Row, string ColumnName)
		{
			return (byte[])Rows[Row][ColumnName];
		}
		public object[] ReadAllValuesFromField(string columnName)
		{
			object[] obj = new object[Count];

			for (int i = 0; i < Count; i++)
				obj[i] = Rows[i][columnName];

			return obj;
		}

		//Handling exception...^^
		public T ToOrDefault<T>(DataRow Row, string ColumnName)
			where T : IConvertible
		{
			try
			{
				return (T)Convert.ChangeType(Row[ColumnName], typeof(T), CultureInfo.InvariantCulture);
			}
			catch (InvalidCastException ex)
			{
				DatabaseLog.Write(ex, "Invalid cast (ColumName {0} : {1})", ColumnName, Row[ColumnName]);
			}
			catch (FormatException ex)
			{
				DatabaseLog.Write(ex, "Invalid Format (ColumName {0} : {1})", ColumnName, Row[ColumnName]);
			}
			catch (OverflowException ex)
			{
				DatabaseLog.Write(ex, "Overflowed (ColumName {0} : {1})", ColumnName, Row[ColumnName]);
			}
			catch (ArgumentException ex)
			{
				DatabaseLog.Write(ex, "Invalid Argument (ColumName {0} : {1})", ColumnName, Row[ColumnName]);
			}
			return default(T);
		}
	}
}
