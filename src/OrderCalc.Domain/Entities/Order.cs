using OrderCalc.Domain.Shared.Entity;
using OrderCalc.Domain.Enums;
using OrderCalc.Domain.Constants;

namespace OrderCalc.Domain.Entities;

public class Order : EntityBase
{
    public int CustomerId { get; private set; }
    public decimal TaxValue { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    protected Order() { }

    private Order(int id, int customerId)
    {
        CustomerId = customerId;
        Status = OrderStatus.Created;
        SetCreated(id);
    }

    public static Order Create(int id,int customerId)
    {
        return new Order(id, customerId);
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        _items.Add(orderItem);
    }
}
