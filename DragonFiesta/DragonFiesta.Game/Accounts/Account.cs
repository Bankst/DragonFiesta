#region

using System;
using DragonFiesta.Database.SQL;

#endregion

namespace DragonFiesta.Game.Accounts
{
    [Serializable]
    public class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }

        private string mPassword = null;

        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                mPassword = MD5Password.CalculateMD5Hash(value);
            }
        }

        public string EMail { get; set; }

        public DateTime CreationDate { get; private set; }
        public string CreationIP { get; private set; }
        public bool IsActivated { get; set; }
        public bool IsBanned { get => (BanDate.AddMinutes(BanTime).Ticks >= DateTime.Now.Ticks); }
        public bool IsOnline { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastIP { get; set; }
        public byte RoleID { get; set; }
        public bool IsAdmin { get { return (RoleID > 0); } }
        public DateTime BanDate { get; set; }
        public long BanTime { get; set; }
        public string BanReason { get; set; }

        public Account(SQLResult pRes, int i)
        {
            RefreshFromSQLDataReader(this, pRes, i);
        }

        public Account(int ID, string Name, string EMail, string Password, DateTime Time, string UserIP, bool IsActivated, DateTime BanDate, long BanTime, bool IsOnline, byte RoleID)
        {
            this.ID = ID;
            this.Name = Name;
            this.EMail = EMail;
            this.mPassword = Password;
            CreationDate = Time;
            CreationIP = UserIP;
            this.IsActivated = IsActivated;
            this.IsOnline = IsOnline;
            LastLogin = Time;
            LastIP = UserIP;
            this.RoleID = RoleID;
            this.BanDate = BanDate;
            this.BanTime = BanTime;
        }

        public static void RefreshFromSQLDataReader(Account Account, SQLResult pRes, int i)
        {
            Account.ID = pRes.Read<int>(i, "ID");
            Account.Name = pRes.Read<string>(i, "Name");
            Account.EMail = pRes.Read<string>(i, "EMail");
            Account.mPassword = pRes.Read<string>(i, "Password");
            Account.CreationDate = pRes.Read<DateTime>(i, "CreationDate");
            Account.CreationIP = pRes.Read<string>(i, "CreationIP");
            Account.IsActivated = pRes.Read<bool>(i, "IsActivated");
            Account.IsOnline = pRes.Read<bool>(i, "IsOnline");
            Account.LastLogin = pRes.Read<DateTime>(i, "LastLogin");
            Account.LastIP = pRes.Read<string>(i, "LastIP");
            Account.RoleID = pRes.Read<byte>(i, "RoleID");
            Account.BanDate = pRes.Read<DateTime>(i, "BanDate");
            Account.BanTime = pRes.Read<long>(i, "BanTime");
            Account.BanReason = pRes.Read<string>(i, "BanReason");
        }

        #region Operator

        public override bool Equals(object obj)
        {
            return (this == (obj as Account));
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode()
                + RoleID.GetHashCode()
                + Password.GetHashCode()
                + EMail.GetHashCode()
                + IsActivated.GetHashCode()
                + IsBanned.GetHashCode()
                + IsOnline.GetHashCode()
                + BanDate.GetHashCode()
                + BanTime.GetHashCode();
        }

        #endregion Operator
    }
}