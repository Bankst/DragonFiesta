//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DragonFiesta.Database.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string CreationIP { get; set; }
        public bool IsActivated { get; set; }
        public bool IsBanned { get; set; }
        public bool IsOnline { get; set; }
        public System.DateTime LastLogin { get; set; }
        public string LastIP { get; set; }
        public byte RoleID { get; set; }
        public byte RegionID { get; set; }
        public System.DateTime BanDate { get; set; }
        public long BanTime { get; set; }
        public string BanReason { get; set; }
    }
}
