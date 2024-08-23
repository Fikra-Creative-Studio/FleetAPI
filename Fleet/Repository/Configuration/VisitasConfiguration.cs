using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class VisitasConfiguration : BaseConfiguration, IEntityTypeConfiguration<Visitas>
    {
        public void Configure(EntityTypeBuilder<Visitas> builder)
        {
            builder.Property(x => x.Observacao).HasColumnType("text");
            builder.Property(x => x.Supervior).HasMaxLength(255);
        }
    }
}
