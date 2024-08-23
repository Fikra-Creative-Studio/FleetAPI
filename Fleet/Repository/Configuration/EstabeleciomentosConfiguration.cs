using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fleet.Enums;

namespace Fleet.Repository.Configuration
{
    public class EstabeleciomentosConfiguration : BaseConfiguration, IEntityTypeConfiguration<Estabelecimentos>
    {
        public void Configure(EntityTypeBuilder<Estabelecimentos> builder)
        {
            builder.Property(x => x.Cnpj).HasMaxLength(255);
            builder.Property(x => x.Razao).HasMaxLength(255);
            builder.Property(x => x.Fantasia).HasMaxLength(255);
            builder.Property(x => x.Telefone).HasMaxLength(255);
            builder.Property(x => x.Cep).HasMaxLength(255);
            builder.Property(x => x.Rua).HasMaxLength(255);
            builder.Property(x => x.Numero).HasMaxLength(255);
            builder.Property(x => x.Bairro).HasMaxLength(255);
            builder.Property(x => x.Cidade).HasMaxLength(255);
            builder.Property(x => x.Estado).HasMaxLength(255);
            builder.Property(x => x.Email).HasMaxLength(255);
        }
    }
}
