using OrderCalc.Domain.Shared;

namespace OrderCalc.Domain.Entities;

public class OrderItem : EntityBase
{
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    protected OrderItem() { }

    public OrderItem(int productId, int quantity, decimal price)
    {
        EntityValidator.GreaterThanZero(quantity, nameof(quantity));

        EntityValidator.NotNegative(price, nameof(price));

        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}
