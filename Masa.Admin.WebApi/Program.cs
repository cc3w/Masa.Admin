using Masa.Admin.WebApi.Extensions;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// If this service does not need to call other services, you can delete the following line.
/*builder.Services.AddDaprClient();*/


builder.Services.AddSerilog();
builder.Services.AddSwagger();
builder.Services.AddMasaFramework();
builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();


/*app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});*/

app.MapControllers();

app.Run();