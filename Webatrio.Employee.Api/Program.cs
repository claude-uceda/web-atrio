using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Json;
using System.Reflection;
using Webatrio.Employee.Api.Modules;
using Webatrio.Employee.Api.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration.MinimumLevel.Debug().WriteTo.Console(new JsonFormatter()).Filter.ByExcluding(Matching.FromSource("Microsoft"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddUserSecrets(typeof(IModule).Assembly);
builder.Services.AddAutoMapper(typeof(AutomapProfile));
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//

app.Run();
