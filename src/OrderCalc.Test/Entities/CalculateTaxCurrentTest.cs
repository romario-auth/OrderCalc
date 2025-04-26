using OrderCalc.Domain.Entities;

namespace OrderCalc.Test.Entities;

public class CalculateTaxCurrentTest
{
    public sealed class Calculate : CalculateTaxCurrentTest
    {
        [Fact]
        public void CalculateTaxNew_ShouldReturnThirtyPercent()
        {
            // Arrange
            CalculateTaxCurrent calculateTaxNew = new CalculateTaxCurrent { };
            decimal total = 100.00m;
            decimal percentTaxExpected = 0.30m;

            // Act
            decimal totalTaxl = calculateTaxNew.Calculate(total);

            // Assert
            Assert.Equal(percentTaxExpected, totalTaxl/total);
        }            
    }
}