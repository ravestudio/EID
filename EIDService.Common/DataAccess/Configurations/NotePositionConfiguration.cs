﻿using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class NotePositionConfiguration : EntityTypeConfiguration<NotePosition>
    {
        public NotePositionConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Count).IsRequired();

                HasRequired(p => p.Deal).WithMany(d => d.NotePositions);
                HasRequired(p => p.DiaryNote).WithMany(n => n.NotePositions);

            }).ToTable("NotePositionSet");
        }
    }
}
