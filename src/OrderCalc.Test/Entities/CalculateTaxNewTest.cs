using OrderCalc.Domain.Entities;

namespace OrderCalc.Test.Entities;

public class CalculateTaxNewTest
{
    public sealed class Calculate : CalculateTaxNewTest
    {
        [Fact]
        public void CalculateTaxNew_ShouldReturnTwentyPercent()
        {
            // Arrange
            CalculateTaxNew calculateTaxNew = new CalculateTaxNew { };
            decimal total = 100.00m;
            decimal percentTaxExpected = 0.20m;

            // Act
            decimal totalTaxl = calculateTaxNew.Calculate(total);

            // Assert
            Assert.Equal(percentTaxExpected, totalTaxl/total);
        }              
    }
}