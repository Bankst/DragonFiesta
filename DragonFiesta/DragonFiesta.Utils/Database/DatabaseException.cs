using System;

namespace DragonFiesta.Utils.Database
{
    [Serializable]
    public class DatabaseException : Exception
    {
        internal DatabaseException(string sMessage) : base(sMessage)
        {
        }
    }
}