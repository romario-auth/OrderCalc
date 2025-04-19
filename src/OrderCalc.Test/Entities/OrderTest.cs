using OrderCalc.Domain.Entities;
using OrderCalc.Test.Shared.Fixtures.Entities;

namespace OrderCalc.Test.Entities;

public class OrderTest
{
    [Fact]
    public void AddOrderItem_WhenCalled_ShouldAddItemToOrder()
    {
        // Arange
        Order order = OrderFixtures.New().Builder();

        // Act
        order.AddOrderItem(OrderItemFixtures.New().Builder());
    
        // Assert
        Assert.NotNull(order.Items);
    }
}
