namespace OrderCalc.Domain.Interfaces;

public interface IConsumer
{
    Task StartConsumingAsync(CancellationToken cancellationToken);
}
