﻿using EIDService.Common.Entities;
using System.Data.Entity.ModelConfiguration;

namespace EIDService.Common.DataAccess.Configurations
{
    public class SecurityConfiguration : EntityTypeConfiguration<Security>
    {
        public SecurityConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();
                Property(p => p.Key).HasColumnName("Key");

                Property(p => p.Code).HasColumnName("Code").IsRequired();
                Property(p => p.Name).HasColumnName("Name").IsRequired();
                Property(p => p.IssueSize).HasColumnName("IssueSize").IsRequired();

                Property(p => p.IssueSize).HasColumnName("IssueSize").IsRequired();
                Property(p => p.IssueSize).HasColumnName("IssueSize").IsRequired();

                Property(p => p.MinStep).HasColumnName("MinStep").IsRequired();
                Property(p => p.LotSize).HasColumnName("LotSize").IsRequired();
                Property(p => p.AlgoTrade).HasColumnName("AlgoTrade").IsRequired();

                Property(p => p.DealSize).HasColumnName("DealSize").IsRequired();

                HasRequired(p => p.Emitent).WithMany(e => e.Securities).Map(mp => mp.MapKey("EmitentId"));

            }).ToTable("SecuritySet");
        }
    }
}
