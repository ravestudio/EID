using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class TradeSessionConfiguration : EntityTypeConfiguration<TradeSession>
    {
        public TradeSessionConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();
                Property(p => p.Key).HasColumnName("Key");
                Property(p => p.Date).HasColumnName("Date").IsRequired();



            }).ToTable("TradeSessionSet");
        }
    }
}
