using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace DFEngine.Database
{
	/// <summary>
	/// Represents a database server and holds information about the database host, port and access credentials.
	/// </summary>
	public class DatabaseServer
	{
		#region Fields

		#endregion Fields

		#region Properties

		/// <summary>
		/// The network host of the database server, eg 'localhost' or '127.0.0.1'.
		/// </summary>
		internal string Host { get; }

		/// <summary>
		/// The username to use when connecting to the database.
		/// </summary>
		internal string User { get; }

		/// <summary>
		/// The password to use in combination with the username when connecting to the database.
		/// </summary>
		internal string Password { get; }

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Constructs a DatabaseServer object with given details.
		/// </summary>
		/// <param name="sHost">The network host of the database server, eg 'localhost' or '127.0.0.1'.</param>
		/// <param name="sUser">The username to use when connecting to the database.</param>
		/// <param name="sPassword">The password to use in combination with the username when connecting to the database.</param>
		internal DatabaseServer(string sHost, string sUser, string sPassword)
		{
			if (string.IsNullOrEmpty(sHost))
				throw new ArgumentException("sHost is null or empty");
			if (string.IsNullOrEmpty(sUser))
				throw new ArgumentException("sUser is null or empty");

			Host = sHost;

			User = sUser;
			Password = sPassword ?? "";
		}

		#endregion Constructor

		#region Methods

		public override string ToString()
		{
			return User + "@" + Host;
		}

		#endregion Methods
	}
}
