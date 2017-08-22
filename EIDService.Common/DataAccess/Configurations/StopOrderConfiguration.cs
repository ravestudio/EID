using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class StopOrderConfiguration : EntityTypeConfiguration<StopOrder>
    {
        public StopOrderConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);
                //Ignore(p => p.StateType);

                Property(p => p.Number).HasColumnName("Number").IsRequired();
                //Property(p => p.BaseOrder).HasColumnName("BaseOrder").IsOptional();
                Property(p => p.OrderNumber).HasColumnName("OrderNumber").IsOptional();

                Property(p => p.Code).HasColumnName("Code").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Time).HasColumnName("Time").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Operation).HasColumnName("Operation").HasMaxLength(50).HasColumnType("varchar").IsRequired();

                Property(p => p.OrderOperation).HasColumnName("OrderOperation").IsRequired();

                Property(p => p.Account).HasColumnName("Account").HasMaxLength(50).HasColumnType("varchar").IsRequired();

                Property(p => p.OrderType).HasColumnName("OrderType").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Price).HasColumnName("Price").IsRequired();
                Property(p => p.StopPrice).HasColumnName("StopPrice").IsRequired();
                Property(p => p.StopLimitPrice).HasColumnName("StopLimitPrice").IsRequired();
                Property(p => p.Count).HasColumnName("Count").IsRequired();

                Property(p => p.State).HasColumnName("State").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.OrderState).HasColumnName("OrderState").IsRequired();

                Property(p => p.Class).HasColumnName("Class").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Client).HasColumnName("Client").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Result).HasColumnName("Result").HasMaxLength(100).HasColumnType("varchar").IsOptional();

            }).ToTable("StopOrderSet");
        }
    }
}
