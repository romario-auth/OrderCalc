using OrderCalc.Domain.Constants;
using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Enums;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Interfaces.Repositories;
using OrderCalc.Domain.Interfaces.Services;

namespace OrderCalc.Domain.Services;
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> Get(int id, CancellationToken cancellationToken)
    {
        return await _orderRepository.Get(id, cancellationToken);
    }

    public async Task<Order> Create(Order order, CancellationToken cancellationToken)
    {
        var orderExist = await _orderRepository.Get(order.Id, cancellationToken);
        if (orderExist != null)
            throw new ArgumentException(string.Format(ErrorMessages.DuplicateOrder));

        _orderRepository.Create(order);
        await _unitOfWork.Commit(cancellationToken);

        return order;
    }

    public async Task<List<Order>> GetByStatus(OrderStatus status, CancellationToken cancellationToken)
    {
        return await _orderRepository.GetByStatus(status, cancellationToken);
    }


    public async Task<Order> CalculateTaxAsync(int id, CancellationToken cancellationToken)
    {
        Order order = await _orderRepository.Get(id, cancellationToken);
        if (order == null)
            throw new ArgumentException($"Order {id} not found.");

        // TODO: Implementar as classes e a lógica de cálculo referentes à Reforma Tributária
        var taxRate = 0.00m;
        if(order.UseTaxReform)
            taxRate = 0.20m;
        else
            taxRate = 0.30m;

        order.SetTaxValue(order.Items.Select(x => x.Price).Sum() * taxRate);
        order.SetTaxStatus(OrderStatus.Calculated);

        _orderRepository.Update(order);
        await _unitOfWork.Commit(cancellationToken);

        return order;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
