
using System.Reflection;
using AD419.Jobs.PullNifaData.Attributes;

namespace AD419.Jobs.PullNifaData.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Get properties in the order they are declared via <seealso cref="OrderAttribute"/>
    /// </summary>
    public static IEnumerable<PropertyInfo> GetOrderedProperties(this Type type)
    {
        return type.GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(OrderAttribute)))
            .OrderBy(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false).Single()).Order);
    }
}