﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiteDesign.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Site_DesignDBEntities : DbContext
    {
        public Site_DesignDBEntities()
            : base("name=Site_DesignDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<SiteUser> SiteUsers { get; set; }
    }
}
