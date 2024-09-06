using System.Reflection;

namespace Masa.Admin.WebApi.Infrastructure;

public class MasaAdminDbContext : MasaDbContext
{

    public MasaAdminDbContext(MasaDbContextOptions<MasaAdminDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreatingExecuting(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreatingExecuting(builder);
    }
}