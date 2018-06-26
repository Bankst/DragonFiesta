using System;

namespace DragonFiesta.Game.Characters.Data
{
    public class ClientLoginInfo
    {
        public DateTime LastLogin { get; set; }

        public bool IsFirstLogin { get; set; }

        public virtual bool IsOnline { get; set; }

        public virtual int AccountID { get; set; }

        public virtual bool RefreshFromSQL(SQLResult pRes, int i)
        {
            try
            {
                AccountID = pRes.Read<int>(i, "AccountID");
                LastLogin = pRes.Read<DateTime>(i, "LastLogin");
                IsOnline = pRes.Read<bool>(i, "IsOnline");
                IsFirstLogin = pRes.Read<bool>(i, "IsFirstLogin");

                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Failed Load ClientLoginInfo.");
                return false;
            }
        }
    }
}