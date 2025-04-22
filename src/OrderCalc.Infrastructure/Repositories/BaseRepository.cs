using Microsoft.EntityFrameworkCore;
using OrderCalc.Domain.Interfaces.Repositories;
using OrderCalc.Domain.Shared.Entity;
using OrderCalc.Infrastructure.Context;

namespace OrderCalc.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(T entity)
    {
        entity.SetCreated(entity.Id);
        _context.Add(entity);
    }
    public void Update(T entity)
    {
        entity.SetUpdated();
        _context.Update(entity);
    }

    public void Delete(T entity)
    {
        entity.SetUpdated();
        _context.Remove(entity);
    }

    public virtual async Task<T> Get(int id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }
}
