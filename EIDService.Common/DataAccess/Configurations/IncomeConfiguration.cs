using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class IncomeConfiguration : EntityTypeConfiguration<Income>
    {
        public IncomeConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Date).IsRequired();

                Property(p => p.Value).IsRequired();
                

            }).ToTable("IncomeSet");
        }
    }
}
