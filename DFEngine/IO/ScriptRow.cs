using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.IO
{
	/// <summary>
	/// Class that represents a row in a script file.
	/// </summary>
	public class ScriptRow
	{
		/// <summary>
		/// The array of values in the row.
		/// </summary>
		private readonly string[] _values;

		/// <summary>
		/// Creates a new instance of the <see cref="ScriptRow"/> class.
		/// </summary>
		/// <param name="values"></param>
		public ScriptRow(string[] values)
		{
			_values = values;
		}

		/// <summary>
		/// Returns the value at the specified index.
		/// </summary>
		/// <param name="index">The index being searched for.</param>
		public string this[int index] => _values[index];
	}
}
