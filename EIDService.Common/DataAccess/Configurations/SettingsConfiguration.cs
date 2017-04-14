using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class SettingsConfiguration: EntityTypeConfiguration<Settings>
    {
        public SettingsConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();
                Property(p => p.Key).HasColumnName("Key");

                Ignore(p => p.ModeType);

                Property(p => p.Mode).IsRequired();
                Property(p => p.TestDateTime).IsRequired();

            }).ToTable("Settings");
        }
    }
}
