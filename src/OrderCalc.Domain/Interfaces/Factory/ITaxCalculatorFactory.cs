using OrderCalc.Domain.Interfaces.Entities;

namespace OrderCalc.Domain.Interfaces.Factory;

public interface ITaxCalculatorFactory
{
    ICalculateTax Create(bool useTaxReform);
}
