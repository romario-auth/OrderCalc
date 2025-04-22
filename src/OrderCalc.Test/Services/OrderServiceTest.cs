using Moq;
using OrderCalc.Domain.Constants;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Interfaces.Repositories;
using OrderCalc.Domain.Services;
using OrderCalc.Test.Shared.Fixtures.Entities;

namespace OrderCalc.Test.Services;
public class OrderServiceTest
{
    [Fact]
    public async Task Create_WhenOrderAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var existingOrder = OrderFixtures.New().Builder();

        var mockRepository = new Mock<IOrderRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockRepository.Setup(repo => repo.Get(existingOrder.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingOrder);

        var service = new OrderService(mockRepository.Object, mockUnitOfWork.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            service.Create(existingOrder, CancellationToken.None));

        // Assert
        Assert.Equal(ErrorMessages.DuplicateOrder, exception.Message);
    }
}