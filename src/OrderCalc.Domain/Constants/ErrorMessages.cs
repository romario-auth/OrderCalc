namespace OrderCalc.Domain.Constants;

public static class ErrorMessages
{
public const string MustBeGreaterThanZero = "{0} deve ser maior que zero.";
public const string MustNotBeNegative = "{0} não deve ser negativo.";
public const string TheOrderMustHaveAtLeastOneItem = "O pedido deve ter pelo menos um item.";
public const string DuplicateOrder = "Pedido duplicado.";
}
