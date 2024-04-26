
using System.Reflection;
using AD419.Jobs.PullNifaData.Attributes;
using FastMember;

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

    /// <summary>
    /// Fast alternative to <seealso cref="PropertyInfo.GetValue(object?)"/>
    /// </summary>
    public static object FastGetValue(this PropertyInfo propertyInfo, object obj)
    {
        // TypeAccessors are cached, so it's okay to call this in tight loops
        var accessor = TypeAccessor.Create(propertyInfo.DeclaringType);
        return accessor[obj, propertyInfo.Name];
    }
}