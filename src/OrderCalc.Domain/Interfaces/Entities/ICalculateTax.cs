namespace OrderCalc.Domain.Interfaces.Entities;

public interface ICalculateTax
{
    decimal Calculate(decimal totalAmount);
}
