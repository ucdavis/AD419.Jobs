using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AD419.Jobs.PullNifaData.Extensions;

public static class PropertyInfoExtensions
{
    public static bool GetIsRequired(this PropertyInfo propertyInfo)
    {
        return propertyInfo.GetCustomAttribute<RequiredAttribute>() != null;    
    }

    public static int GetMaxLength(this PropertyInfo propertyInfo)
    {
        var attribute = propertyInfo.GetCustomAttribute<MaxLengthAttribute>();
        if (attribute == null)
            return 50; // seems to be a common default
        return attribute.Length;
    }

    public static Type GetUnwrappedType(this PropertyInfo propertyInfo)
    {
        // SqlBulkCopy's type conversion doesn't understand Nullable<> types, so unwrap them
        var type = propertyInfo.PropertyType;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Nullable.GetUnderlyingType(type)!;
        }
        return type;
    }

    public static string GetSqlType(this PropertyInfo propertyInfo)
    {
        var type = propertyInfo.GetUnwrappedType();
        return type switch
        {
            Type t when t == typeof(string) => $"VARCHAR({propertyInfo.GetMaxLength()})",
            Type t when t  == typeof(DateTime) => "DATE",
            Type t when t  == typeof(int) => "INT",
            Type t when t  == typeof(decimal) => "DECIMAL(18, 2)",
            Type t when t  == typeof(bool) => "BIT",
            _ => throw new NotSupportedException($"Unsupported type {type.Name}")
        };
    }
}