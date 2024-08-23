using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class ListasItensConfiguration : BaseConfiguration, IEntityTypeConfiguration<ListasItens>
    {
        public void Configure(EntityTypeBuilder<ListasItens> builder)
        {
            builder.Property(x => x.Titulo).HasMaxLength(255);
            builder.Property(x => x.Descrição).HasMaxLength(255);
        }
    }
}
