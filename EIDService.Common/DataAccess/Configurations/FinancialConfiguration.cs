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

                Property(p => p.Revenue).HasColumnName("Revenue").IsRequired();
                Property(p => p.OperatingIncome).HasColumnName("OperatingIncome").IsRequired();
                Property(p => p.NetIncome).HasColumnName("NetIncome").IsRequired();

                Property(p => p.CurrentAssets).HasColumnName("CurrentAssets").IsRequired();
                Property(p => p.FixedAssets).HasColumnName("FixedAssets").IsRequired();

                Property(p => p.CurrentLiabilities).HasColumnName("CurrentLiabilities").IsRequired();
                Property(p => p.LongTermLiabilities).HasColumnName("LongTermLiabilities").IsRequired();

                Property(p => p.FlowOperatingActivities).HasColumnName("FlowOperatingActivities").IsRequired();
                Property(p => p.ChangesInAssets).HasColumnName("ChangesInAssets").IsRequired();
                Property(p => p.FlowOperatingTaxesPaid).HasColumnName("FlowOperatingTaxesPaid").IsRequired();

                Property(p => p.FlowInvestingActivities).HasColumnName("FlowInvestingActivities").IsRequired();
                Property(p => p.FlowFinancingActivities).HasColumnName("FlowFinancingActivities").IsRequired();

                Property(p => p.StockIssuance).HasColumnName("StockIssuance").IsRequired();
                Property(p => p.DividendsPaid).HasColumnName("DividendsPaid").IsRequired();
                Property(p => p.EarningsPerShare).HasColumnName("EarningsPerShare").IsRequired();

                Property(p => p.Price).HasColumnName("Price").IsRequired();

                HasRequired(p => p.Emitent).WithMany(e => e.Financials).Map(mp => mp.MapKey("EmitentId"));

            }).ToTable("FinancialSet");
        }
    }
}
