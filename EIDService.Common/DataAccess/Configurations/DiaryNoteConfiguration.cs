using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess.Configurations
{
    public class DiaryNoteConfiguration : EntityTypeConfiguration<DiaryNote>
    {
        public DiaryNoteConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();

                Ignore(p => p.Key);

                Property(p => p.Code).IsRequired();

                Property(p => p.NoteType).IsRequired();

                Property(p => p.Open).IsOptional();
                Property(p => p.Close).IsOptional();

                Property(p => p.OpenPrice).IsOptional();
                Property(p => p.ClosePrice).IsOptional();

                Property(p => p.OpenValue).IsOptional();
                Property(p => p.CloseValue).IsOptional();

                Property(p => p.Count).IsRequired();

            }).ToTable("DiaryNote");
        }
    }
}
