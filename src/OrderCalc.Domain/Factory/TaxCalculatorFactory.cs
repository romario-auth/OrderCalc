using OrderCalc.Domain.Entities;
using OrderCalc.Domain.Interfaces.Entities;
using OrderCalc.Domain.Interfaces.Factory;

namespace OrderCalc.Domain.Factory;

public class TaxCalculatorFactory : ITaxCalculatorFactory
{
    public ICalculateTax Create(bool useTaxReform)
    {
        return useTaxReform
            ? new CalculateTaxNew()
            : new CalculateTaxCurrent();
    }
}
