using OrderCalc.Domain.Entities;

namespace OrderCalc.Test.Shared.Fixtures.Entities;

public class OrderFixtures
{
    private int _customerId = 1;
    private int _id = 1;
    private bool _useTaxReform = false;

    public static OrderFixtures New() => new OrderFixtures();

    public Order Builder()
    {
        return Order.Create(_id, _customerId, _useTaxReform);
    }

    public OrderFixtures WithUseTaxReform(bool useTaxReform)
    {
        _useTaxReform = useTaxReform;
        return this;
    }

    public Order BuilderWithTwoOrderItems(bool useTaxReform)
    {
        Order order = OrderFixtures.New().WithUseTaxReform(useTaxReform).Builder();
        order.AddOrderItem(OrderItemFixtures.New().Complete(1, 2, 100.0m).Builder());
        order.AddOrderItem(OrderItemFixtures.New().Complete(2, 3, 50.0m).Builder());

        return order;
    }
}