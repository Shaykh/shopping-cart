using Basket.Application;
using Basket.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    Log.Information("Starting Basket API");

    // Use Serilog for logging
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Application layer
    builder.Services.AddApplication();

    // Add Infrastructure layer
    builder.Services.AddInfrastructure(builder.Configuration);

    // Add Health Checks
    builder.Services.AddHealthChecks()
        .AddRedis(
            redisConnectionString: builder.Configuration.GetSection("CacheSettings:ConnectionString").Value!,
            name: "redis",
            tags: ["redis", "cache"]);

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API v1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseHttpsRedirection();

    // Add global exception handler
    app.UseMiddleware<Basket.API.Middleware.GlobalExceptionHandlerMiddleware>();

    app.UseAuthorization();
    app.MapControllers();

    // Health Check endpoint
    app.MapHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    Log.Information("Basket API is running...");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
