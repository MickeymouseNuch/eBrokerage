﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MASDBEntities : DbContext
    {
        public MASDBEntities()
            : base("name=MASDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AmphurTable> AmphurTables { get; set; }
        public virtual DbSet<BankTable> BankTables { get; set; }
        public virtual DbSet<BankTypeTable> BankTypeTables { get; set; }
        public virtual DbSet<DistrictTable> DistrictTables { get; set; }
        public virtual DbSet<ProvinceTable> ProvinceTables { get; set; }
        public virtual DbSet<ReportForAutoMailTable> ReportForAutoMailTables { get; set; }
        public virtual DbSet<ReportCriteriaTable> ReportCriteriaTables { get; set; }
        public virtual DbSet<ReportTable> ReportTables { get; set; }
        public virtual DbSet<STG_EMPLOYEEVw> STG_EMPLOYEEVw { get; set; }
        public virtual DbSet<STG_EMPLOYEEVw_ALL> STG_EMPLOYEEVw_ALL { get; set; }
    }
}