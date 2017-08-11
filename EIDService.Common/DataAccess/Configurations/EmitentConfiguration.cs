using EIDService.Common.Entities;
using System.Data.Entity.ModelConfiguration;

namespace EIDService.Common.DataAccess.Configurations
{
    public class EmitentConfiguration : EntityTypeConfiguration<Emitent>
    {
        public EmitentConfiguration()
        {
            Map(m =>
            {
                HasKey(p => p.Id);
                Property(p => p.Id).HasColumnName("Id").IsRequired();
                Property(p => p.Key).HasColumnName("Key");
                Property(p => p.Name).HasColumnName("Name").IsRequired();

                Property(p => p.Description).HasColumnName("Description").IsOptional();
                Property(p => p.WebSite).HasColumnName("WebSite").IsOptional();



            }).ToTable("EmitentSet");
        }
    }
}
