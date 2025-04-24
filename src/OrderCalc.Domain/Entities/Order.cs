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
    public bool UseTaxReform { get; private set; }

    protected Order() { }

    private Order(int id, int customerId, bool useTaxReform)
    {
        CustomerId = customerId;
        Status = OrderStatus.Created;
        UseTaxReform = useTaxReform;
        SetCreated(id);
    }

    public static Order Create(int id,int customerId, bool UseTaxReform)
    {
        return new Order(id, customerId, UseTaxReform);
    }

    public void SetTaxValue(decimal taxValue)
    {
        TaxValue = taxValue;
    }

    public void SetTaxStatus(OrderStatus status)
    {
        Status = status;
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        _items.Add(orderItem);
    }
}
