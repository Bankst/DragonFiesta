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
    
    public partial class GoldSpammer
    {
        public int ReporterID { get; set; }
        public string ReporterIP { get; set; }
        public int SpammerID { get; set; }
        public string SpammerIP { get; set; }
        public System.DateTime ReportDate { get; set; }
    }
}