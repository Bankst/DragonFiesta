using System;

namespace DragonFiesta.Utils.Database
{
    /// <summary>
    /// Represents a storage database.
    /// </summary>
    public class Database
    {
        #region Fields

        private readonly string mName;
        private readonly int mMinPoolSize;
        private readonly int mMaxPoolSize;
        private readonly int mPoolLifeTime;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The name of the database to connect to.
        /// </summary>
        internal string Name
        {
            get { return mName; }
        }

        /// <summary>
        /// The minimum connection pool size for the database.
        /// </summary>
        internal int MinPoolSize
        {
            get { return mMinPoolSize; }
        }

        /// <summary>
        /// The maximum connection pool size for the database.
        /// </summary>
        internal int ClientLifeTime
        {
            get { return mPoolLifeTime; }
        }

        /// <summary>
        /// The maximum connection pool size for the database.
        /// </summary>
        internal int MaxPoolSize
        {
            get { return mMaxPoolSize; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructs a Database instance with given details.
        /// </summary>
        /// <param name="sName">The name of the database.</param>
        /// <param name="minPoolSize">The minimum connection pool size for the database.</param>
        /// <param name="maxPoolSize"> The maximum connection pool size for the database.</param>
        internal Database(string sName, int minPoolSize, int maxPoolSize, int _mPoolLifeTime)
        {
            if (sName == null || sName.Length == 0)
                throw new ArgumentException(sName);

            mName = sName;
            mMinPoolSize = minPoolSize;
            mMaxPoolSize = maxPoolSize;
            mPoolLifeTime = _mPoolLifeTime;
        }

        #endregion Constructor
    }
}