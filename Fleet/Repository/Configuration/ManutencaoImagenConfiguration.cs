using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class ManutencaoImagenConfiguration : BaseConfiguration, IEntityTypeConfiguration<ManutencaoImagens>
    {
        public void Configure(EntityTypeBuilder<ManutencaoImagens> builder)
        {
            builder.Property(x => x.Url).HasMaxLength(255);
        }
    }
}
