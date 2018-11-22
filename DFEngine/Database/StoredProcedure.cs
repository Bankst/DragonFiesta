using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DFEngine.Database
{
	/// <summary>
	/// Class to call stored procedures from a SQL database.
	/// </summary>
	public class StoredProcedure : Object
	{
		/// <summary>
		/// The underlying SQL command object.
		/// </summary>
		private readonly SqlCommand command;
		/// <summary>
		/// The name of the stored procedure.
		/// </summary>
		private readonly string name;
		/// <summary>
		/// Parameters that will be modified when the command is executed.
		/// </summary>
		private readonly Dictionary<string, object> outputParameters;

		/// <summary>
		/// Creates a new instance of the <see cref="StoredProcedure"/> class.
		/// </summary>
		/// <param name="name">The name of the stored procedure.</param>
		/// <param name="connection">The connection that the procedure is being ran on.</param>
		public StoredProcedure(string name, SqlConnection connection)
		{
			this.name = name;

			command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;

			outputParameters = new Dictionary<string, object>();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="StoredProcedure"/> class.
		/// </summary>
		/// <param name="name">The name of the stored procedure.</param>
		/// <param name="connection">The connection that the procedure is being ran on.</param>
		public StoredProcedure(string name, DatabaseClient client)
		{
			this.name = name;

			command = client.mConnection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;

			outputParameters = new Dictionary<string, object>();
		}

		/// <summary>
		/// Adds an output parameter to the procedure.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="size">The data length of the value.</param>
		public void AddOutput<T>(string name, int size = -1)
		{
			if (outputParameters.ContainsKey(name))
			{
				return;
			}

			var parameter = command.Parameters.AddWithValue(name, default(T));
			parameter.Direction = ParameterDirection.Output;

			if (typeof(T) == typeof(byte[]))
			{
				parameter.DbType = DbType.Binary;
			}

			if (size != -1)
			{
				parameter.Size = size;
			}

			outputParameters.Add(name, default(T));
		}

		/// <summary>
		/// Adds a parameter to the procedure.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value to pass to the procedure.</param>
		/// <param name="size">The data length of the value.</param>
		public void AddParameter(string name, object value, int size = -1)
		{
			var parameter = command.Parameters.AddWithValue(name, value ?? DBNull.Value);

			if (size != -1)
			{
				parameter.Size = size;
			}
		}

		/// <summary>
		/// Finalizes the output variables before returning from the query.
		/// </summary>
		public void FinalizeOutput()
		{
			for (var i = 0; i < outputParameters.Count; i++)
			{
				var output = outputParameters.ElementAt(i);
				outputParameters[output.Key] = command.Parameters[output.Key].Value;
			}
		}

		/// <summary>
		/// Gets a value from the output parameters.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="name">The name of the parameter.</param>
		public T GetOutput<T>(string name)
		{
			if (!outputParameters.ContainsKey(name) || outputParameters[name] is DBNull)
			{
				return default(T);
			}

			return (T)outputParameters[name];
		}

		/// <summary>
		/// Runs the command.
		/// </summary>
		public StoredProcedure Run()
		{
			SetCommandText();
			command.ExecuteNonQuery();
			FinalizeOutput();
			return this;
		}

		/// <summary>
		/// Runs the command and returns an <see cref="OdbcDataReader"/> instance.
		/// </summary>
		public SqlDataReader RunReader()
		{
			SetCommandText();
			var reader = command.ExecuteReader();
			FinalizeOutput();
			return reader;
		}

		/// <summary>
		/// Sets the command's text.
		/// </summary>
		public void SetCommandText()
		{
			command.CommandText = $"{{call {name}";

			if (command.Parameters.Count > 0)
			{
				command.CommandText += "(";

				for (var i = 0; i < command.Parameters.Count; i++)
				{
					command.CommandText += "?, ";
				}

				command.CommandText = command.CommandText.Substring(0, command.CommandText.LastIndexOf(", ", StringComparison.Ordinal));
				command.CommandText += ")";
			}

			command.CommandText += "}";
		}

		/// <summary>
		/// Destroys the <see cref="StoredProcedure"/> instance.
		/// </summary>
		protected override void Destroy()
		{
			command.Dispose();
		}
	}
}
