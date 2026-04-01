using System.Text.Json.Serialization;
using BelgradeATC.Application.Commands;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Services;
using BelgradeATC.Core.Interfaces;
using BelgradeATC.Core.Interfaces.Repositories;
using BelgradeATC.Infrastructure;
using BelgradeATC.Infrastructure.Auth;
using BelgradeATC.Infrastructure.BackgroundServices;
using BelgradeATC.Infrastructure.Data;
using BelgradeATC.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();
builder.Services.AddScoped<IStateChangeLogRepository, StateChangeLogRepository>();
builder.Services.AddHostedService<GroundCrewService>();
builder.Services.AddSingleton<IWeatherStore, WeatherStore>();
builder.Services.AddHostedService<WeatherService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDbSeeder, DbSeeder>();

// Services
builder.Services.AddScoped<IAircraftService, AircraftService>();

// MediatR — scans Application assembly for all handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UpdateLocationCommand).Assembly));

builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, AircraftAuthHandler>(
        AircraftAuthHandler.SchemeName, _ => { });

builder.Services.AddAuthentication("AdminCookie")
.AddCookie("AdminCookie", options =>
{
    options.LoginPath = "/Login";
});


builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
    await seeder.Process();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
