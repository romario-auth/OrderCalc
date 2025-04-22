using OrderCalc.Domain.Shared.Entity;

namespace OrderCalc.Domain.Entities;

public class OrderItem : EntityBase
{
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    protected OrderItem() { }

    public OrderItem(int productId, int quantity, decimal price)
    {
        EntityValidator.GreaterThanZero(quantity, nameof(quantity));

        EntityValidator.NotNegative(price, nameof(price));

        Quantity = quantity;
        Price = price;

        SetCreated(productId);
    }
}
