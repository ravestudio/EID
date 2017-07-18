using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class TransactionResultConfiguration : EntityTypeConfiguration<TransactionResult>
    {

        public TransactionResultConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Record).HasColumnName("Record").IsRequired();
                Property(p => p.Processed).HasColumnName("Processed").IsRequired();

            }).ToTable("TransactionResultSet");
        }
    }
}
