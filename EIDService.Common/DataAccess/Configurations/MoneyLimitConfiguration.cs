using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class MoneyLimitConfiguration : EntityTypeConfiguration<MoneyLimit>
    {
        public MoneyLimitConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Firm).HasColumnName("Firm").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.MoneyGroup).HasColumnName("MoneyGroup").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Client).HasColumnName("Client").HasMaxLength(20).HasColumnType("varchar").IsRequired();
                Property(p => p.Type).HasColumnName("Type").HasMaxLength(10).HasColumnType("varchar").IsRequired();

                Property(p => p.Available).HasColumnName("Available").IsRequired();


            }).ToTable("MoneyLimitSet");
        }
    }
}
