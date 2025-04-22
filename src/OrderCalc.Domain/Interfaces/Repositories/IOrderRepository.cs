using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Enums;

namespace OrderCalc.Domain.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<List<Order>> GetByStatus(OrderStatus status, CancellationToken cancellationToken);
}
