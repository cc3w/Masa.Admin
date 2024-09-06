using Masa.Admin.WebApi.Infrastructure;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.WithProperty("Application", "XFree.SimpleService")
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// If this service does not need to call other services, you can delete the following line.
builder.Services.AddDaprClient();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();
builder.Services
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<Program>();
    });
builder.Services.AddMasaDbContext<MasaAdminDbContext>(options =>
{
    options.UseSqlite("DataSource=masaApp.db");
});
/*builder.Services.AddEventBus(eventBusBuilder =>
{
    eventBusBuilder.UseMiddleware(typeof(ValidatorMiddleware<>));
    eventBusBuilder.UseMiddleware(typeof(LogMiddleware<>));
});*/

/*builder.Services.AddAutoInject();*/
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    #region MigrationDb
    using var context = app.Services.CreateScope().ServiceProvider.GetService<MasaAdminDbContext>();
    {
        context!.Database.EnsureCreated();
    }
    #endregion
}

app.UseHttpsRedirection();
app.UseRouting();


// Used for Dapr Pub/Sub.
app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});

app.MapControllers();

app.Run();