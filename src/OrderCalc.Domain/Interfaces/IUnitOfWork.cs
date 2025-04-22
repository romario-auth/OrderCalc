namespace OrderCalc.Domain.Interfaces;
public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}