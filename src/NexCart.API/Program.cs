using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using NexCart.API.Middleware;
using NexCart.Application;
using NexCart.Infrastructure;
using NexCart.Infrastructure.Persistence;
using NexCart.Infrastructure.Persistence.Seed;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("🚀 Starting NexCart API...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // --- Register Services ---
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    // CORS for Next.js frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
            policy.WithOrigins("http://localhost:3000", "http://nexcart-web:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod());
    });

    builder.Services.AddHealthChecks();

    var app = builder.Build();

    // --- Middleware Pipeline ---
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.UseCors("AllowFrontend");
    app.MapControllers();
    app.MapHealthChecks("/health");

    // --- Database: Auto-migrate & Seed ---
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<NexCartDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await DataSeeder.SeedAsync(dbContext);
        Log.Information("✅ Database ready with seed data");
    }

    Log.Information("✅ NexCart API is running at http://localhost:5119");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
