using OrderCalc.Domain.Common;

namespace OrderCalc.Domain.Entities;

public class OrderItem : EntityBase
{
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    protected OrderItem() { }

    public OrderItem(int productId, int quantity, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (price < 0)
            throw new ArgumentException("Price must be greater than zero.");

        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}
