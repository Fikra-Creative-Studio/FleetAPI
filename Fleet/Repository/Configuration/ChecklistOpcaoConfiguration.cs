using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class ChecklistOpcaoConfiguration : BaseConfiguration, IEntityTypeConfiguration<ChecklistOpcao>
    {
        public void Configure(EntityTypeBuilder<ChecklistOpcao> builder)
        {
            builder.Property(x => x.Titulo).HasMaxLength(255);
            builder.Property(x => x.Descricao).HasMaxLength(255);
        }
    }
}
