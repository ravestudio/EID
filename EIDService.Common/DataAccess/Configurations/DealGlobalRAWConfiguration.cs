using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class DealGlobalRAWConfiguration : EntityTypeConfiguration<DealGlobalRAW>
    {
        public DealGlobalRAWConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Code).HasColumnName("Code").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Time).HasColumnName("Time").HasMaxLength(10).HasColumnType("varchar").IsRequired();

                Property(p => p.Date).HasColumnName("Date").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Operation).HasColumnName("Operation").HasMaxLength(50).HasColumnType("varchar").IsRequired();

                Property(p => p.Price).HasColumnName("Price").IsRequired();
                Property(p => p.Count).HasColumnName("Count").IsRequired();
                Property(p => p.Volume).HasColumnName("Volume").IsRequired();

            }).ToTable("DealGlobalRAWSet");
        }

    }
}
