using Masa.Admin.WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// If this service does not need to call other services, you can delete the following line.
/*builder.Services.AddDaprClient();*/


builder.Services.AddSerilog();
builder.Services.AddSwagger();
builder.Services.AddAllowCors();
builder.Services.AddMasaFramework();
builder.Services.AddControllers();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();