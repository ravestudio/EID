using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class PositionConfiguration : EntityTypeConfiguration<Position>
    {
        public PositionConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Firm).HasColumnName("Firm").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.SecurityName).HasColumnName("SecurityName").HasMaxLength(100).HasColumnType("varchar").IsRequired();
                Property(p => p.Code).HasColumnName("Code").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Account).HasColumnName("Account").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Client).HasColumnName("Client").HasMaxLength(20).HasColumnType("varchar").IsRequired();
                Property(p => p.Type).HasColumnName("Type").HasMaxLength(10).HasColumnType("varchar").IsRequired();

                Property(p => p.IncomingBalance).HasColumnName("IncomingBalance").IsRequired();
                Property(p => p.IncomingLimit).HasColumnName("IncomingLimit").IsRequired();

                Property(p => p.CurrentBalance).HasColumnName("CurrentBalance").IsRequired();
                Property(p => p.CurrentLimit).HasColumnName("CurrentLimit").IsRequired();

                Property(p => p.Blocked).HasColumnName("Blocked").IsRequired();
                Property(p => p.BlockedForPurchase).HasColumnName("BlockedForPurchase").IsRequired();

                Property(p => p.Total).HasColumnName("Total").IsRequired();
                Property(p => p.Available).HasColumnName("Available").IsRequired();

                Property(p => p.Balance).HasColumnName("Balance").IsRequired();
                Property(p => p.PurchasePrice).HasColumnName("PurchasePrice").IsRequired();

            }).ToTable("PositionSet");
        }
    }
}
