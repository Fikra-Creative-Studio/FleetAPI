using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class VeiculosConfiguration : BaseConfiguration, IEntityTypeConfiguration<Veiculos>
    {
        public void Configure(EntityTypeBuilder<Veiculos> builder)
        {
            builder.Property(x => x.Marca).HasMaxLength(255);
            builder.Property(x => x.Modelo).HasMaxLength(255);
            builder.Property(x => x.Ano).HasMaxLength(255);
            builder.Property(x => x.Placa).HasMaxLength(255);
            builder.Property(x => x.Combustivel).HasMaxLength(255);
            builder.Property(x => x.Odometro).HasMaxLength(255);
        }
    }
}
