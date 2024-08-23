using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class VisitaOpcaoConfiguration : BaseConfiguration, IEntityTypeConfiguration<VisitaOpcao>
    {
        public void Configure(EntityTypeBuilder<VisitaOpcao> builder)
        {
            builder.Property(x => x.Titulo).HasMaxLength(255);
            builder.Property(x => x.Descricao).HasMaxLength(255);
        }
    }
}
