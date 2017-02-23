using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class FinancialConfiguration : EntityTypeConfiguration<Financial>
    {
        public FinancialConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();
                Property(p => p.Key).HasColumnName("Key");

                Property(p => p.Year).HasColumnName("Year").IsRequired();
                Property(p => p.Period).HasColumnName("Period").IsRequired();

                Property(p => p.Revenue).HasColumnName("Revenue").IsOptional();
                Property(p => p.OperatingExpenses).HasColumnName("OperatingExpenses").IsOptional();
                Property(p => p.Expenses).HasColumnName("Expenses").IsOptional();
                Property(p => p.OperatingIncome).HasColumnName("OperatingIncome").IsOptional();
                Property(p => p.NetIncome).HasColumnName("NetIncome").IsOptional();
                Property(p => p.CurrentAssets).HasColumnName("CurrentAssets").IsOptional();
                Property(p => p.FixedAssets).HasColumnName("FixedAssets").IsOptional();
                Property(p => p.Equity).HasColumnName("Equity").IsOptional();
                Property(p => p.CurrentLiabilities).HasColumnName("CurrentLiabilities").IsOptional();
                Property(p => p.LongTermLiabilities).HasColumnName("LongTermLiabilities").IsOptional();

                HasRequired(p => p.Emitent).WithMany(e => e.Financials).Map(mp => mp.MapKey("EmitentId"));

            }).ToTable("FinancialSet");
        }
    }
}
