using Masa.Admin.Domain.Entities;

namespace Masa.Admin.WebApi.Infrastructure.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tb_User");
            builder.HasKey(x => x.Id);
        }
    }
}
