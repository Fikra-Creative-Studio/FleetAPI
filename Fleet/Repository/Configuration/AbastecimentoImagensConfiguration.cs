using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Repository.Configuration
{
    public class AbastecimentoImagensConfiguration : BaseConfiguration, IEntityTypeConfiguration<AbastecimentoImagens>
    {
        public void Configure(EntityTypeBuilder<AbastecimentoImagens> builder)
        {
            builder.Property(x => x.Url).HasMaxLength(255);
        }
    }
}
