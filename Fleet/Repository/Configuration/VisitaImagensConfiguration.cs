using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class VisitaImagensConfiguration : BaseConfiguration, IEntityTypeConfiguration<VisitaImagens>
    {
        public void Configure(EntityTypeBuilder<VisitaImagens> builder)
        {
            builder.Property(x => x.Url).HasMaxLength(255);
        }
    }
}
