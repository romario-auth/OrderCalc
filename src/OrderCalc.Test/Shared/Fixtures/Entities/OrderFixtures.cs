using OrderCalc.Domain.Entities;

namespace OrderCalc.Test.Shared.Fixtures.Entities;

public class OrderFixtures
{
    private int _customerId = 1;

    public static OrderFixtures New() => new OrderFixtures();

    public Order Builder()
    {
        return Order.Create(_customerId);
    }
}