using Microsoft.Extensions.DependencyInjection;
using OrderCalc.Application.Interfaces;
using OrderCalc.Application.Service;
using OrderCalc.Domain.Factory;
using OrderCalc.Domain.Interfaces.Factory;
using OrderCalc.Domain.Interfaces.Services;
using OrderCalc.Domain.Services;


namespace OrderCalc.Application.Extensions;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services)
    {
        services.AddScoped<IOrderServiceAplication, OrderServiceAplication>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITaxCalculatorFactory, TaxCalculatorFactory>(); 
    }
}
