using OrderCalc.Domain.Entities;

namespace OrderCalc.Test.Shared.Fixtures.Entities;

public class OrderItemFixtures
{
    private int _productId = 1;
    private int _quantity = 1;
    private decimal _price = 5.55m;

    public static OrderItemFixtures New() => new OrderItemFixtures();

    public OrderItem Builder()
    {
        return new OrderItem(_productId, _quantity, _price);
    }

    public OrderItemFixtures Complete(int productId, int quantity, decimal price)
    {
        _productId = productId;
        _quantity = quantity;
        _price = price;

        return this;
    }

}