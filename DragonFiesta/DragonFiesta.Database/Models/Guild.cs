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
    
    public partial class Guild
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public bool AllowGuildWar { get; set; }
        public string Message { get; set; }
        public System.DateTime MessageCreateDate { get; set; }
        public int MessageCreaterID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public long Money { get; set; }
    }
}
