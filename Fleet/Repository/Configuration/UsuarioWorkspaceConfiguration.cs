using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class UsuarioWorkspaceConfiguration : BaseConfiguration, IEntityTypeConfiguration<UsuarioWorkspace>
    {
        public void Configure(EntityTypeBuilder<UsuarioWorkspace> builder)
        {
            builder.ToTable("usuarioworkspace");

            builder.HasOne(u => u.Usuario).WithMany(x => x.UsuarioWorkspaces);
            builder.HasOne(u => u.Workspace).WithMany(x => x.UsuarioWorkspaces);

        }
    }
}
