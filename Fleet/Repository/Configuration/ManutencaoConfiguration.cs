using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class ManutencaoConfiguration : BaseConfiguration, IEntityTypeConfiguration<Manutencao>
    {
        public void Configure(EntityTypeBuilder<Manutencao> builder)
        {
            builder.Property(x => x.Odometro).HasMaxLength(255);
            builder.Property(x => x.Servicos).HasMaxLength(255);
            builder.Property(x => x.Observacoes).HasMaxLength(255);
        }
    }
}
