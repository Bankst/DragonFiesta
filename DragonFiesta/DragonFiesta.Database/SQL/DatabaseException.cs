using System;

namespace DragonFiesta.Database.SQL
{
    [Serializable]
    public class DatabaseException : Exception
    {
        internal DatabaseException(string sMessage) : base(sMessage)
        {
        }
    }
}