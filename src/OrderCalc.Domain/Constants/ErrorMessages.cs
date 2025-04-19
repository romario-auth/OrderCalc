namespace OrderCalc.Domain.Constants;

public static class ErrorMessages
{
    public const string MustBeGreaterThanZero = "{0} must be greater than zero.";
    public const string MustNotBeNegative = "{0} must not be negative.";
    public const string CannotDowngradeOrderStatus = "Cannot downgrade the order status.";
}
