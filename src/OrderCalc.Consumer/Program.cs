using Microsoft.EntityFrameworkCore;
using OrderCalc.Consumer;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Interfaces.Repositories;
using OrderCalc.Domain.Interfaces.Services;
using OrderCalc.Domain.Services;
using OrderCalc.Domain.Settings;
using OrderCalc.Infrastructure.Context;
using OrderCalc.Infrastructure.Repositories;
using OrderCalc.Infrastructure.Services;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQ"));

        services.AddHostedService<Worker>();
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IConsumer, Consumer>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
    })
    .UseSerilog()
    .Build();
host.Run();
