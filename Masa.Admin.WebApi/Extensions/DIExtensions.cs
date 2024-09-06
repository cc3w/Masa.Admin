using Masa.Admin.WebApi.Infrastructure;

namespace Masa.Admin.WebApi.Extensions;

public static class DIExtensions
{
    #region Masa
    public static void AddMasaFramework(this IServiceCollection services)
    {
        //注册MasaDbContext
        services.AddMasaDbContext<MasaAdminDbContext>(optionsBuilder => optionsBuilder.UseMySQL());
    }

    #endregion
}
