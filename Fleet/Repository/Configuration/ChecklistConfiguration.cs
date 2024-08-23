using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Repository.Configuration
{
    public class ChecklistConfiguration : BaseConfiguration, IEntityTypeConfiguration<Checklist> 
    {
        public void Configure(EntityTypeBuilder<Checklist> builder)
        {
            builder.Property(x => x.OdometroRetirada).HasMaxLength(255);
            builder.Property(x => x.OdometroDevolucao).HasMaxLength(255);
            builder.Property(x => x.ObsRetirada).HasColumnType("text");
            builder.Property(x => x.ObsDevolucao).HasColumnType("text");
            builder.Property(x => x.Avaria).HasMaxLength(255);
            builder.Property(x => x.OsbAvaria).HasColumnType("text");
        }

    }
}
