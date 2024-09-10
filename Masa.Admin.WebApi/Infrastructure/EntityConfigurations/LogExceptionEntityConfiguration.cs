using Masa.Admin.Domain.Entities;

namespace Masa.Admin.WebApi.Infrastructure.EntityConfigurations
{
    public class LogExceptionEntityConfiguration : IEntityTypeConfiguration<LogException>
    {
        public void Configure(EntityTypeBuilder<LogException> builder)
        {
            builder.ToTable("Sys_LogException");
            builder.HasKey(x => x.Id);
        }
    }
}
