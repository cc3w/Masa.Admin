using Masa.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masa.Admin.WebApi.Infrastructure.EntityConfigurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Sys_Role");
            builder.HasKey(x => x.Id); 
        }
    }
}
