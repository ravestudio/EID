﻿using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
            //this.Database.Delete();
            Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = true;

            this.Configuration.AutoDetectChangesEnabled = true;

            //Database.SetInitializer(new Initializer());
        }

        public DbSet<Emitent> Emitent { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<Financial> Financials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configurations.EmitentConfiguration());
            modelBuilder.Configurations.Add(new Configurations.SecurityConfiguration());
            modelBuilder.Configurations.Add(new Configurations.FinancialConfiguration());
        }
    }
}