using OrderCalc.Domain.Interfaces.Entities;

namespace OrderCalc.Domain.Entities;

public class CalculateTaxNew : ICalculateTax
{
    public decimal Calculate(decimal totalValue) => totalValue * 0.2m;
}