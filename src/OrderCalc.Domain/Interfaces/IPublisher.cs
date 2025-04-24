namespace OrderCalc.Domain.Interfaces;

public interface IPublisher
{
    void Publish<T>(T message, string routingKey);
}