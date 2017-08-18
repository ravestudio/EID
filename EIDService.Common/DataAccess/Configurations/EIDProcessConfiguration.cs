using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class EIDProcessConfiguration : EntityTypeConfiguration<EIDProcess>
    {
        public EIDProcessConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Type).HasColumnName("Type").IsRequired();
                Property(p => p.Status).HasColumnName("Status").IsRequired();

                Property(p => p.Data).HasColumnName("Data").IsRequired();

            }).ToTable("EIDProcessSet");
        }
    }
}
