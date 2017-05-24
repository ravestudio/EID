using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class CandleConfiguration : EntityTypeConfiguration<Candle>
    {
        public CandleConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).IsRequired();

                Property(p => p.Code).HasColumnName("Code").HasMaxLength(10).HasColumnType("varchar")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Code"))).IsRequired();

                Ignore(p => p.Key);

                Property(p => p.CandleDate).HasColumnName("CandleDate").HasMaxLength(30).HasColumnType("varchar").IsRequired();
                Property(p => p.CandleTime).HasColumnName("CandleTime").HasMaxLength(30).HasColumnType("varchar").IsRequired();

                Property(p => p.OpenPrice).HasColumnName("OpenPrice").IsRequired();
                Property(p => p.ClosePrice).HasColumnName("ClosePrice").IsRequired();
                Property(p => p.MaxPrice).HasColumnName("MaxPrice").IsRequired();
                Property(p => p.MinPrice).HasColumnName("MinPrice").IsRequired();

                Property(p => p.Value).HasColumnName("Value").HasPrecision(18, 0).IsRequired();
                Property(p => p.Volume).HasColumnName("Volume").HasPrecision(18,0).IsRequired();

            }).ToTable("CandleSet");
        }
    }
}
