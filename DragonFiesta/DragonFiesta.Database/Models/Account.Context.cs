﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AccountEntity : DbContext
    {
        public AccountEntity()
            : base("name=AccountEntity")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<IPBlock> IPBlocks { get; set; }
        public virtual DbSet<RegionServer> RegionServers { get; set; }
        public virtual DbSet<Version> Versions { get; set; }
        public virtual DbSet<WorldList> WorldLists { get; set; }
    }
}
