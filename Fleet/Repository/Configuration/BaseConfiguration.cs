using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Repository.Configuration
{
    public class BaseConfiguration
    {
        public void Configure(EntityTypeBuilder<DbEntity> builder)
        {
            builder.Property(x => x.Id).UseMySqlIdentityColumn();
            builder.Property(x => x.Ativo).HasDefaultValue(true);
        }
    }
}