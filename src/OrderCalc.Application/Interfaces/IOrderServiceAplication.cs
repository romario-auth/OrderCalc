using OrderCalc.Domain.Model.DTO;

namespace OrderCalc.Application.Interfaces;

public interface IOrderServiceAplication : IDisposable
{
    Task<OrderResponse> Get(int id, CancellationToken cancellationToken);
    Task<OrderResponse> Create(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken);
    Task<List<OrderResponse>> GetByStatus(string status, CancellationToken cancellationToken);
}