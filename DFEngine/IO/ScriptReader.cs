using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.IO
{
	/// <summary>
	/// Class that reads data from a <see cref="Script"/> file.
	/// </summary>
	public class ScriptReader : IDisposable
	{
		/// <summary>
		/// Returns true if the script has rows to read.
		/// </summary>
		public bool HasRows => _rows.Count > 0;

		/// <summary>
		/// The current row that we are reading.
		/// </summary>
		private ScriptRow _currentRow;
		/// <summary>
		/// The queued rows to read.
		/// </summary>
		private readonly ConcurrentQueue<ScriptRow> _rows;

		/// <summary>
		/// Creates a new instance of the <see cref="ScriptReader"/> class.
		/// </summary>
		/// <param name="rows">The rows to read.</param>
		public ScriptReader(List<ScriptRow> rows)
		{
			if (rows == null)
			{
				this._rows = new ConcurrentQueue<ScriptRow>();
				return;
			}

			this._rows = new ConcurrentQueue<ScriptRow>(rows);
		}

		/// <summary>
		/// Disposes the reader.
		/// </summary>
		public void Dispose()
		{
			// empty...
		}

		/// <summary>
		/// Gets a boolean from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public bool GetBoolean(int index)
		{
			return bool.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets a byte from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public byte GetByte(int index)
		{
			return byte.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets a 16-bit integer from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public short GetInt16(int index)
		{
			return short.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets a 32-bit integer from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public int GetInt32(int index)
		{
			return int.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets a 64-bit integer from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public long GetInt64(int index)
		{
			return long.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets a signed byte from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public sbyte GetSByte(int index)
		{
			return sbyte.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets an unsigned 16-bit integer from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public ushort GetUInt16(int index)
		{
			return ushort.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets an unsigned 32-bit integer from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public uint GetUInt32(int index)
		{
			return uint.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets an unsigned 64-bit integer from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public ulong GetUInt64(int index)
		{
			return ulong.Parse(_currentRow[index]);
		}

		/// <summary>
		/// Gets a string from the row.
		/// </summary>
		/// <param name="index">The index to get the value from.</param>
		public string GetString(int index)
		{
			return _currentRow[index];
		}

		/// <summary>
		/// Tries to read a row from the file.
		/// </summary>
		/// <returns>True if the row was read successfully.</returns>
		public bool Read()
		{
			return _rows.TryDequeue(out _currentRow) && _currentRow != null;
		}
	}
}
