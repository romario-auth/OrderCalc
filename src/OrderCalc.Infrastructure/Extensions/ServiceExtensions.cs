using OrderCalc.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderCalc.Domain.Interfaces.Repositories;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Infrastructure.Repositories;

namespace OrderCalc.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void InfrastructureServiceExtensions(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("OrdersDb"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}