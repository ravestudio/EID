using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class TransactionConfiguration : EntityTypeConfiguration<Transaction>
    {
        public TransactionConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Name).HasColumnName("Name").IsRequired();
                Property(p => p.Status).HasColumnName("Status").IsRequired();
                Property(p => p.Description).HasColumnName("Description").IsOptional();
                Property(p => p.OrderNumber).HasColumnName("OrderNumber").IsOptional();
                Property(p => p.Processed).HasColumnName("Processed").IsRequired();

            }).ToTable("TransactionSet");
        }
    }
}
