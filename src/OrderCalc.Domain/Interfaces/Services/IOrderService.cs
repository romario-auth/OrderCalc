using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Enums;

namespace OrderCalc.Domain.Interfaces.Services;

public interface IOrderService : IDisposable
{
    Task<Order> Get(int id, CancellationToken cancellationToken);
    Task<Order> Create(Order order, CancellationToken cancellationToken);
    Task<List<Order>> GetByStatus(OrderStatus status, CancellationToken cancellationToken);
}