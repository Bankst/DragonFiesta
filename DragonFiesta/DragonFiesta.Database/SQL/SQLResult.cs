using System;
using System.Data;
using System.Globalization;

namespace DragonFiesta.Database.SQL
{
	public class SQLResult : DataTable
	{
		public int Count { get; set; }

		public T Read<T>(int row, string columnName, int number = 0)
			where T : IConvertible
		{
			return ToOrDefault<T>(Rows[row], columnName + (number != 0 ? (1 + number).ToString() : ""));
		}

		public bool HasValues => !Count.Equals(0);

		public byte[] GetBytes(int row, string columnName)
		{
			return (byte[])Rows[row][columnName];
		}
		public object[] ReadAllValuesFromField(string columnName)
		{
			object[] obj = new object[Count];

			for (int i = 0; i < Count; i++)
				obj[i] = Rows[i][columnName];

			return obj;
		}

		//Handling exception...^^
		public T ToOrDefault<T>(DataRow row, string columnName)
			where T : IConvertible
		{
			try
			{
				return (T)Convert.ChangeType(row[columnName], typeof(T), CultureInfo.InvariantCulture);
			}
			catch (InvalidCastException ex)
			{
				DatabaseLog.Write(ex, "Invalid cast (ColumName {0} : {1})", columnName, row[columnName]);
			}
			catch (FormatException ex)
			{
				DatabaseLog.Write(ex, "Invalid Format  (ColumName {0} : {1})", columnName, row[columnName]);
			}
			catch (OverflowException ex)
			{
				DatabaseLog.Write(ex, "Overflowed   (ColumName {0} : {1})", columnName, row[columnName]);
			}
			catch (ArgumentException ex)
			{
				DatabaseLog.Write(ex, "Invalid Argument  (ColumName {0} : {1})", columnName, row[columnName]);
			}
			return default(T);
		}
	}
}