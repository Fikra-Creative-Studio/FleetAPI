using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Repository.Configuration
{
    public class AbastecimentoConfiguration : BaseConfiguration, IEntityTypeConfiguration<Abastecimento>
    {
        public void Configure(EntityTypeBuilder<Abastecimento> builder)
        {
            builder.Property(x => x.Data);
            builder.Property(x => x.Odometro).HasMaxLength(255);
            builder.Property(x => x.Observacoes).HasMaxLength(1000);
        }
    }
}