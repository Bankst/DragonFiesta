using System;

namespace DragonFiesta.Database.SQL
{
    /// <summary>
    /// Represents a database server and holds information about the database host, port and access credentials.
    /// </summary>
    public class DatabaseServer
    {
        #region Fields

        private readonly string mHost;

        private readonly string mUser;
        private readonly string mPassword;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The network host of the database server, eg 'localhost' or '127.0.0.1'.
        /// </summary>
        internal string Host
        {
            get { return mHost; }
        }

        /// <summary>
        /// The username to use when connecting to the database.
        /// </summary>
        internal string User
        {
            get { return mUser; }
        }

        /// <summary>
        /// The password to use in combination with the username when connecting to the database.
        /// </summary>
        internal string Password
        {
            get { return mPassword; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructs a DatabaseServer object with given details.
        /// </summary>
        /// <param name="sHost">The network host of the database server, eg 'localhost' or '127.0.0.1'.</param>
        /// <param name="Port">The network port of the database server as an unsigned 32 bit integer.</param>
        /// <param name="sUser">The username to use when connecting to the database.</param>
        /// <param name="sPassword">The password to use in combination with the username when connecting to the database.</param>
        internal DatabaseServer(string sHost, string sUser, string sPassword)
        {
            if (sHost == null || sHost.Length == 0)
                throw new ArgumentException("sHost is null or empty");
            if (sUser == null || sUser.Length == 0)
                throw new ArgumentException("sUser is null or empty");

            mHost = sHost;

            mUser = sUser;
            mPassword = (sPassword != null) ? sPassword : "";
        }

        #endregion Constructor

        #region Methods

        public override string ToString()
        {
            return mUser + "@" + mHost;
        }

        #endregion Methods
    }
}