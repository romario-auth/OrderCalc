using Moq;
using OrderCalc.Domain.Constants;
using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Factory;
using OrderCalc.Domain.Services;
using OrderCalc.Infrastructure.Context;
using OrderCalc.Infrastructure.Repositories;
using OrderCalc.Test.Shared.Fixtures.EF;
using OrderCalc.Test.Shared.Fixtures.Entities;

namespace OrderCalc.Test.Services;

public class OrderServiceTest
{
    private AppDbContext _context;
    private OrderRepository _repository;
    private UnitOfWork _unitOfWork;
    private Mock<TaxCalculatorFactory> _mockTaxCalculatorFactory;

    public OrderServiceTest()
    {

        _context = TestDBContext.GetInMemoryDbContext(Guid.NewGuid().ToString());
        _repository = new OrderRepository(_context);
        _unitOfWork = new UnitOfWork(_context);
        _mockTaxCalculatorFactory = new Mock<TaxCalculatorFactory>();
    }

    public sealed class Create : OrderServiceTest
    {

        [Fact]
        public async Task Create_WhenOrderAlreadyExists_ShouldThrowArgumentException()
        {
            // Arrange
            var existingOrder = OrderFixtures.New().Builder();
            var duplicatedOrder = OrderFixtures.New().Builder();
            
            _context.Order.Add(existingOrder);
            await _context.SaveChangesAsync();

            var _orderService = new OrderService(_repository, _unitOfWork, _mockTaxCalculatorFactory.Object);

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _orderService.Create(duplicatedOrder, CancellationToken.None));

            // Assert
            Assert.Equal(ErrorMessages.DuplicateOrder, exception.Message);
        }
    }

    public sealed class CalculateTaxAsync : OrderServiceTest
    {
        public CalculateTaxAsync() {}

        [Fact]
        public async Task WhenFeatureFlagIsActiveToUseTaxReform__ShouldUseCalculateTaxNew()
        {
            // Arrange
            var service = new OrderService(_repository, _unitOfWork, _mockTaxCalculatorFactory.Object);

            bool useTaxReform = true;
            Order order = OrderFixtures.New().BuilderWithTwoOrderItems(useTaxReform);

            decimal total = (2 * 100 + 3 * 50m);
            decimal expectedNewTax = 0.2m;

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await service.CalculateTaxAsync(order.Id, CancellationToken.None);

            // Assert
            var updatedOrder = await service.Get(order.Id, CancellationToken.None);
            Assert.Equal(expectedNewTax, updatedOrder.TaxValue / total);
        }

        [Fact]
        public async void WhenFeatureFlagIsNotActiveToUseTaxReform__ShouldUseCalculateTaxCurrent()
        {
            // Arrange
            var service = new OrderService(_repository, _unitOfWork, _mockTaxCalculatorFactory.Object);
            bool notUseTaxReform = false;
            Order order = OrderFixtures.New().BuilderWithTwoOrderItems(notUseTaxReform);

            decimal total = (2 * 100 + 3 * 50m);
            decimal expectedNewTax = 0.3m;

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await service.CalculateTaxAsync(order.Id, CancellationToken.None);

            // Assert
            var updatedOrder = await service.Get(order.Id, CancellationToken.None);
            Assert.Equal(expectedNewTax, updatedOrder.TaxValue / total);
        }
    }
}