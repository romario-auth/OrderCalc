using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OrderCalc.Domain.Shared.Enums;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()?
                        .Name ?? enumValue.ToString();
    }

    public static TEnum? FromDisplayName<TEnum>(string displayName) where TEnum : struct, Enum
    {
        foreach (var value in Enum.GetValues(typeof(TEnum)))
        {
            var enumValue = (Enum)value;
            if (string.Equals(enumValue.GetDisplayName(), displayName, StringComparison.OrdinalIgnoreCase))
            {
                return (TEnum)enumValue;
            }
        }

        return null;
    }
}