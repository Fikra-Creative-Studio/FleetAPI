using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Repository.Configuration
{
    public class ChecklistImagensConfiguration : BaseConfiguration, IEntityTypeConfiguration<ChecklistImagens>
    {
        public void Configure(EntityTypeBuilder<ChecklistImagens> builder)
        {
            builder.Property(x => x.Url).HasMaxLength(255);
        }
    }
}
