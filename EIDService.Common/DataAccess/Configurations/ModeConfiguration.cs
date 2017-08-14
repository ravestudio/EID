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
    public class ModeConfiguration : EntityTypeConfiguration<Mode>
    {
        public ModeConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Firm).HasColumnName("Firm").IsRequired();
                Property(p => p.Account).HasColumnName("Account").IsRequired();
                Property(p => p.Class).HasColumnName("Class").IsRequired();
                Property(p => p.Client).HasColumnName("Client").IsRequired();
                Property(p => p.Active).HasColumnName("Active").IsRequired();

            }).ToTable("ModeSet");
        }
    }
}
