using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Repository.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(x => x.CPF).HasMaxLength(255).IsRequired();

            builder.Property(x => x.Email).HasMaxLength(255)
                                        .IsRequired();

            builder.Property(x => x.Senha).HasMaxLength(255).IsRequired();

            builder.Property(x => x.Token).HasMaxLength(255);

             builder.Property(x => x.Nome).HasMaxLength(255).IsRequired();

            //builder.Property(x => x.Papel).HasDefaultValue(PapelEnum.Usuario)
            //                              .IsRequired();
        }

    }
}