using OrderCalc.Domain.Shared.Entity;

namespace OrderCalc.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : EntityBase
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T> Get(int Id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken);
}