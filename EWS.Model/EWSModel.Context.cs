﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EWS.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EWSContext : DbContext
    {
        public EWSContext()
            : base("name=EWSContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EWSKnownWords> EWSKnownWords { get; set; }
        public virtual DbSet<EWSLearnList> EWSLearnList { get; set; }
        public virtual DbSet<EWSList> EWSList { get; set; }
        public virtual DbSet<EWSListWord> EWSListWord { get; set; }
        public virtual DbSet<EWSReadPart> EWSReadPart { get; set; }
        public virtual DbSet<EWSSampleSentence> EWSSampleSentence { get; set; }
        public virtual DbSet<EWSUser> EWSUser { get; set; }
        public virtual DbSet<EWSWord> EWSWord { get; set; }
        public virtual DbSet<EWSWillLearn> EWSWillLearn { get; set; }
        public virtual DbSet<EWSReadingPartCategory> EWSReadingPartCategory { get; set; }
        public virtual DbSet<EWSGroupItem> EWSGroupItem { get; set; }
        public virtual DbSet<EWSListWillLearn> EWSListWillLearn { get; set; }
    }
}
