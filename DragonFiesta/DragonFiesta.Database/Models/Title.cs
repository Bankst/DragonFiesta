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
    
    public partial class Title
    {
        public long ID { get; set; }
        public int OwnerID { get; set; }
        public int TitleType { get; set; }
        public byte TitleLevel { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        public long Data { get; set; }
    }
}
