using Microsoft.AspNetCore.Mvc;
using OrderCalc.Application.Extensions;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Settings;
using OrderCalc.Infrastructure.Extensions;
using OrderCalc.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

// Ativando o Serilog como logger da aplicação
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.ConfigurePersistenceApp();
builder.Services.InfrastructureServiceExtensions();
builder.Services.AddControllers();

builder.Services.Configure<TaxCalculationSettings>(
    builder.Configuration.GetSection("TaxCalculation"));

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IPublisher, Publisher>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "OrderCalc API - v1",
        Description = "API for order calculation and management"
    });
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

// To do habilitar o HttpRedirection para o deploy
//app.UseHttpsRedirection();
app.MapControllers();

app.Run();
