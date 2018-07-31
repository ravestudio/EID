using EIDService.Common.Entities;
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
        public DbSet<Candle> Candles { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<StopOrder> StopOrders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<MoneyLimit> MoneyLimit { get; set; }
        public DbSet<Mode> Modes { get; set; }
        public DbSet<EIDProcess> EIDprocesses { get; set; }
        public DbSet<DiaryNote> DiaryNotes { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<Income> Incomes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configurations.EmitentConfiguration());
            modelBuilder.Configurations.Add(new Configurations.SecurityConfiguration());
            modelBuilder.Configurations.Add(new Configurations.FinancialConfiguration());
            modelBuilder.Configurations.Add(new Configurations.TradeSessionConfiguration());
            modelBuilder.Configurations.Add(new Configurations.CandleConfiguration());
            modelBuilder.Configurations.Add(new Configurations.SettingsConfiguration());
            modelBuilder.Configurations.Add(new Configurations.OrderConfiguration());
            modelBuilder.Configurations.Add(new Configurations.PositionConfiguration());
            modelBuilder.Configurations.Add(new Configurations.StopOrderConfiguration());
            modelBuilder.Configurations.Add(new Configurations.TransactionConfiguration());
            modelBuilder.Configurations.Add(new Configurations.TransactionResultConfiguration());
            modelBuilder.Configurations.Add(new Configurations.DealConfiguration());
            modelBuilder.Configurations.Add(new Configurations.DealRAWConfiguration());
            modelBuilder.Configurations.Add(new Configurations.MoneyLimitConfiguration());
            modelBuilder.Configurations.Add(new Configurations.ModeConfiguration());
            modelBuilder.Configurations.Add(new Configurations.EIDProcessConfiguration());
            modelBuilder.Configurations.Add(new Configurations.PosTypeConfiguration());
            modelBuilder.Configurations.Add(new Configurations.OrderStateConfiguration());
            modelBuilder.Configurations.Add(new Configurations.OrderOperationConfiguration());
            modelBuilder.Configurations.Add(new Configurations.DiaryNoteConfiguration());
            modelBuilder.Configurations.Add(new Configurations.PortfolioItemConfiguration());
            modelBuilder.Configurations.Add(new Configurations.IncomeConfiguration());
            modelBuilder.Configurations.Add(new Configurations.DealGlobalConfiguration());
            modelBuilder.Configurations.Add(new Configurations.DealGlobalRAWConfiguration());
        }
    }
}
