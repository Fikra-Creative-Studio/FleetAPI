using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class WorkspaceConfiguration : BaseConfiguration, IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.Property(x => x.Cnpj).HasMaxLength(255);
            builder.Property(x => x.Fantasia).HasMaxLength(255);
            builder.Property(x => x.UrlImagem).HasMaxLength(1000).IsRequired();
        }
    }
}
