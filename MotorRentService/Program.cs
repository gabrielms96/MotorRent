#region Mainetence
/*
Comment: Created Mainetence Region and correction mapping.
Created: 08/31/2024 15:00
Author: Gabriel MS
*/
#endregion

using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MotorRentService.Data;
using MotorRentService.Models;
using MotorRentService.Profiles;
using MotorRentService.RabbitMqClient;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/MotorRental-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
var connectionString = builder.Configuration.GetConnectionString("MotorRentConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

// RabbitMQ Client
builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.Internal().MethodMappingEnabled = false, 
    typeof(DeliveryPersonProfile), 
    typeof(MotorcycleProfile), 
    typeof(NotificationProfile), 
    typeof(RentalPlanProfile), 
    typeof(RentalProfile));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotorRentService", Version = "v1" });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.Migrate();
        Log.Information("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Database migration failed");
        throw;
    }
}

Log.Information("Starting MotorRent API");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
