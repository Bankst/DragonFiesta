//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DragonFiesta.Database.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DBItem
    {
        public long ItemKey { get; set; }
        public byte StorageType { get; set; }
        public int Owner { get; set; }
        public short Storage { get; set; }
        public int ItemID { get; set; }
        public int Flags { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual ItemOption ItemOption { get; set; }
        public virtual DBCharacter Character { get; set; }
    }
}