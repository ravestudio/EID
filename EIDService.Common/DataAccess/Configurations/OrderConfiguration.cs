﻿using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Number).HasColumnName("Number").IsRequired();
                Property(p => p.Code).HasColumnName("Code").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Time).HasColumnName("Time").HasMaxLength(10).HasColumnType("varchar").IsRequired();
                Property(p => p.Operation).HasColumnName("Operation").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Account).HasColumnName("Account").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Price).HasColumnName("Price").IsRequired();

                Property(p => p.State).HasColumnName("State").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Class).HasColumnName("Class").HasMaxLength(50).HasColumnType("varchar").IsRequired();
                Property(p => p.Comment).HasColumnName("Comment").HasMaxLength(50).HasColumnType("varchar").IsRequired();

            }).ToTable("OrderSet");
        }
    }
}