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
    
    public partial class Buff
    {
        public long ID { get; set; }
        public int OwnerID { get; set; }
        public int AbStateIndex { get; set; }
        public int Strength { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime ExpireDate { get; set; }
        public string test { get; set; }
    }
}
