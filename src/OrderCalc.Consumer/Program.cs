using OrderCalc.Consumer;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Settings;
using OrderCalc.Infrastructure.Services;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQ"));
        
        services.AddHostedService<Worker>();
        services.AddSingleton<IConsumer, Consumer>();
    })
    .UseSerilog()
    .Build();
host.Run();
