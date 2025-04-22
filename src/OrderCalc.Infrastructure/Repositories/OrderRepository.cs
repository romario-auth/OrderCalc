using Microsoft.EntityFrameworkCore;
using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Enums;
using OrderCalc.Domain.Interfaces.Repositories;
using OrderCalc.Infrastructure.Context;

namespace OrderCalc.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository, IDisposable
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public override async Task<Order> Get(int id, CancellationToken cancellationToken)
    {
        return await _context.Set<Order>().Include("_items").FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Order>> GetByStatus(OrderStatus status, CancellationToken cancellationToken)
    {
        return await _context.Order
            .Include("_items")
            .Where(o => o.Status == status)
            .ToListAsync(cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}


