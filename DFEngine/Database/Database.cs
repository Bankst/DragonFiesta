using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.Database
{
	/// <summary>
	/// Represents a storage database.
	/// </summary>
	public class Database
	{
		#region Fields

		#endregion Fields

		#region Properties

		/// <summary>
		/// The name of the database to connect to.
		/// </summary>
		internal string Name { get; }

		/// <summary>
		/// The minimum connection pool size for the database.
		/// </summary>
		internal int MinPoolSize { get; }

		/// <summary>
		/// The maximum connection pool size for the database.
		/// </summary>
		internal int MaxPoolSize { get; }

		/// <summary>
		/// The maximum connection pool size for the database.
		/// </summary>
		internal int ClientLifeTime { get; }

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Constructs a Database instance with given details.
		/// </summary>
		/// <param name="sName">The name of the database.</param>
		/// <param name="minPoolSize">The minimum connection pool size for the database.</param>
		/// <param name="maxPoolSize">The maximum connection pool size for the database.</param>
		/// <param name="mPoolLifeTime">The lifetime of the connection pool.</param>
		internal Database(string sName, int minPoolSize, int maxPoolSize, int mPoolLifeTime)
		{
			if (string.IsNullOrEmpty(sName))
				throw new ArgumentException(sName);

			Name = sName;
			MinPoolSize = minPoolSize;
			MaxPoolSize = maxPoolSize;
			ClientLifeTime = mPoolLifeTime;
		}

		#endregion Constructor
	}
}
