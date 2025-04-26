using OrderCalc.Domain.Interfaces.Entities;

namespace OrderCalc.Domain.Entities;

public class CalculateTaxCurrent : ICalculateTax
{
    public decimal Calculate(decimal totalValue) => totalValue * 0.3m;
}
