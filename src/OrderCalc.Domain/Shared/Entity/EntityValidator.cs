using OrderCalc.Domain.Constants;

namespace OrderCalc.Domain.Shared.Entity;

public class EntityValidator
{
    public static void GreaterThanZero(decimal value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentException(string.Format(ErrorMessages.MustBeGreaterThanZero, fieldName));
    }

    public static void GreaterThanZero(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentException(string.Format(ErrorMessages.MustBeGreaterThanZero, fieldName));
    }

    public static void NotNegative(decimal value, string fieldName)
    {
        if (value < 0)
            throw new ArgumentException(string.Format(ErrorMessages.MustNotBeNegative, fieldName));
    }

    public static void NotNegative(int value, string fieldName)
    {
        if (value < 0)
            throw new ArgumentException(string.Format(ErrorMessages.MustNotBeNegative, fieldName));
    }
}
