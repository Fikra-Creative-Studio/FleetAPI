using Fleet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository.Configuration
{
    public class UsuarioWorkspaceConfiguration : BaseConfiguration, IEntityTypeConfiguration<UsuarioWorkspace>
    {
        public void Configure(EntityTypeBuilder<UsuarioWorkspace> builder)
        {

        }
    }
}
