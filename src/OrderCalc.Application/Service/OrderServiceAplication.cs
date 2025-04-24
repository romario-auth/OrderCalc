using Microsoft.Extensions.Options;
using OrderCalc.Application.Interfaces;
using OrderCalc.Domain.Model.DTO;
using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Enums;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Interfaces.Services;
using OrderCalc.Domain.Settings;
using OrderCalc.Domain.Shared.Enums;

namespace OrderCalc.Application.Service;

public class OrderServiceAplication : IOrderServiceAplication
{
    private readonly IOrderService _orderService;
    private readonly IOptions<TaxCalculationSettings> _taxSettings;
    private readonly IPublisher _publisher;

    public OrderServiceAplication(IOrderService orderService, IOptions<TaxCalculationSettings> taxSettings, IPublisher publisher)
    {
        _orderService = orderService;
        _taxSettings = taxSettings;
        _publisher = publisher;
    }

    public async Task<OrderResponse> Get(int id, CancellationToken cancellationToken)
    {
        Order order = await _orderService.Get(id, cancellationToken);

        if (order is null)
            return null;

        List<OrderItemResponse> orderItemResponses = order.Items
        .Select(item => new OrderItemResponse(item.Id, item.Quantity, item.Price))
        .ToList();

        OrderResponse orderResponse = new OrderResponse(order.Id, order.CustomerId, order.TaxValue, order.Status.GetDisplayName(), orderItemResponses);

        return orderResponse;
    }

    public async Task<OrderResponse> Create(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken)
    {
        Order order = Order.Create(createOrderRequest.PedidoId, createOrderRequest.ClienteId, _taxSettings.Value.UseTaxReform);
        foreach (CreateOrderItemRequest item in createOrderRequest.Itens)
        {
            OrderItem orderItem = new OrderItem(item.ProdutoId, item.Quantidade, item.Valor);
            order.AddOrderItem(orderItem);
        }

        await _orderService.Create(order, cancellationToken);

        _publisher.Publish(new OrderCreatedMessage { OrderId = order.Id }, "order.created");

        List<OrderItemResponse> orderItemResponses = order.Items
        .Select(item => new OrderItemResponse(item.Id, item.Quantity, item.Price))
        .ToList();

        return new OrderResponse(order.Id, order.CustomerId, order.TaxValue, order.Status.GetDisplayName(), orderItemResponses);
    }

    public async Task<List<OrderResponse>> GetByStatus(string status, CancellationToken cancellationToken)
    {
        var parsedStatus = EnumExtensions.FromDisplayName<OrderStatus>(status);

        if (parsedStatus == null)
            throw new ArgumentException($"Invalid status: {status}");

        List<Order> orders = await _orderService.GetByStatus(parsedStatus.Value, cancellationToken);

        return orders.Select(order => new OrderResponse(
            order.Id,
            order.CustomerId,
            order.TaxValue,
            order.Status.GetDisplayName(),
            order.Items.Select(item => new OrderItemResponse(
                item.Id,
                item.Quantity,
                item.Price
            )).ToList()
        )).ToList();
    }

    public void Dispose()
    {
        _orderService?.Dispose();

        GC.SuppressFinalize(this);
    }
}
