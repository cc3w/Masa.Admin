
using Masa.Admin.WebApi.Infrastructure;
using Masa.BuildingBlocks.Data.UoW;
using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using Masa.BuildingBlocks.Dispatcher.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace Masa.Admin.WebApi.Extensions;

public static class DIExtensions
{

    #region Serilog
    public static void AddSerilog(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.WithProperty("Application", "XFree.SimpleService")
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    #endregion


    #region Swagger
    /// <summary>
    /// Swagger配置
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: Authorization: Bearer {token}",
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                       Array.Empty<string>()
                    }
                });
            try
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // =true时 swaggerui排序将不按照首字母排序
                options.IncludeXmlComments(xmlPath, true);

                var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(t => t.Name!.Contains("Endpoints"));
                foreach (var assemblyName in assemblyNames)
                {
                    xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assemblyName.Name}.xml");
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath, true);
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
            }
            options.CustomOperationIds(apiDesc =>
            {
                var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                return $"{controllerAction?.ControllerName} - {controllerAction?.ActionName}-{controllerAction?.GetHashCode()}";
            });
        });
    }
    #endregion



    #region Masa
    public static void AddMasaFramework(this IServiceCollection services)
    {
        services.AddMasaConfiguration();

        //services.AddAutoInject();

        //自动映射
        services.AddMapster();

        //注册MasaDbContext
        services.AddMasaDbContext<MasaAdminDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseMySql(new MySqlServerVersion("5.7.31"));
            //optionsBuilder.UseSqlServer();
            optionsBuilder.UseFilter();
        });

        //领域事件(进程内事件 工作单元 仓储)
        services.AddDomainEventBus(options =>
        {
            options.UseEventBus(eventBusBuilder =>
            {
                //添加事件验证中间件（Validator）
                eventBusBuilder.UseMiddleware(typeof(ValidatorEventMiddleware<>));
            });
            options.UseUoW<MasaAdminDbContext>();
            options.UseRepository<MasaAdminDbContext>();
        });

        //集成事件跨服务传输，服务的发布与订阅不在同一个进程中
        /*        services.AddIntegrationEventBus(options =>
                {
                    options
                        .UseDapr()//使用Dapr提供pub/sub能力，也可以自行选择其他的
                        .UseEventLog<MasaAdminDbContext>()//使用基于EFCore的本地消息表
                        .UseUoW<MasaAdminDbContext>(); //使用工作单元
                });*/

    }

    #endregion
}
