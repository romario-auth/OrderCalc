using OrderCalc.Domain.Entities;
using OrderCalc.Test.Shared.Fixtures.Entities;
using OrderCalc.Domain.Constants;

namespace OrderCalc.Test.Entities;

public class OrderItemTest
{

    [Fact]
    public void CreatingOrderItem_WithQuantityLessThanToZero_ShouldThrowException()
    {
        // Arrange
        int productId = 1;
        int quantity = -10;
        decimal price = 5.55m;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            OrderItemFixtures.New().Complete(productId, quantity, price).Builder());

        // Assert
        Assert.Equal(string.Format(ErrorMessages.MustBeGreaterThanZero, nameof(quantity)), exception.Message);
    }

    [Fact]
    public void CreatingOrderItem_WithQuantityEqualThanToZero_ShouldThrowException()
    {
        // Arrange
        int productId = 1;
        int quantity = 0;
        decimal price = 5.55m;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            OrderItemFixtures.New().Complete(productId, quantity, price).Builder());

        // Assert
        Assert.Equal(string.Format(ErrorMessages.MustBeGreaterThanZero, nameof(quantity)), exception.Message);
    }

    [Fact]
    public void CreatingOrderItem_WithQuantityGreaterThanToZero_ShouldThrowException()
    {
        // Arrange
        int productId = 1;
        int quantity = 10;
        decimal price = 5.55m;

        // Act
        var exception = Record.Exception(() =>
            OrderItemFixtures.New().Complete(productId, quantity, price).Builder());

        // Assert
        Assert.Null(exception);
    }
        
    [Fact]
    public void CreatingOrderItem_WithPriceLessThanToZero_ShouldThrowException()
    {
        // Arrange
        int productId = 1;
        int quantity = 1;
        decimal price = -5.5m;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            OrderItemFixtures.New().Complete(productId, quantity, price).Builder());

        // Assert
        Assert.Equal(string.Format(ErrorMessages.MustNotBeNegative, nameof(price)), exception.Message);
    }

    [Fact]
    public void CreatingOrderItem_WithPriceEqualToZero_ShouldNotThrowException()
    {
        // Arrange
        int productId = 1;
        int quantity = 1;
        decimal price = 0;

        // Act
        var exception = Record.Exception(() =>
            OrderItemFixtures.New().Complete(productId, quantity, price).Builder());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void CreatingOrderItem_WithValidPrice_ShouldNotThrowException()
    {
        // Arrange
        int productId = 1;
        int quantity = 1;
        decimal price = 1.11m;

        // Act
        var exception = Record.Exception(() =>
            OrderItemFixtures.New().Complete(productId, quantity, price).Builder());

        // Assert
        Assert.Null(exception);
    }
}